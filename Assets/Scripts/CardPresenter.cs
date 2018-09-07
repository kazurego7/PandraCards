using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


public class CardPresenter : MonoBehaviour {
	struct CardInfo {
		public Transform TransformView {
			get;
			private set;
		}
	}

	struct OneHandInfo {
		public OneHand OneHandModel {
			get;
			private set;
		}
		public Transform TransformView {
			get;
			private set;
		}
	}

	struct DeckInfo {
		public Deck DeckModel {
			get;
			private set;
		}
		public Transform TransformView {
			get;
			private set;
		}
	}

	struct PlayAreaInfo {
		public PlayArea PlayAreaModel {
			get;
			private set;
		}
		public Transform TransformView {
			get;
			private set;
		}
	}
	Dictionary<Card, CardInfo> CardInfos {
		get;
		set;
	} = new Dictionary<Card, CardInfo> ();

	IList<OneHandInfo> OneHandInfos {
		get;
		set;
	} = new List<OneHandInfo> ();

	IList<DeckInfo> DeckInfos {
		get;
		set;
	} = new List<DeckInfo> ();

	IList<PlayAreaInfo> PlayAreaInfos {
		get;
		set;
	} = new List<PlayAreaInfo> ();

	void Start () {
		// Subscribeでしか、入力してはいけないよ！

		IObservable<Vector3> CreateLinerMoves (Vector3 source, Vector3 target, int moveingFrame = 1) {
			return Observable
				.IntervalFrame (moveingFrame)
				.Select (t => Vector3.Lerp (source, target, (float) t / moveingFrame));
		}

		IObservable < (Transform, IObservable<Vector3>) > CreateDrawReplenisheObservable (OneHandInfo target) {
			// Noticeを集めて、動くCardのTransformに変換する
			var moveCardTransformObservable = target.OneHandModel.ReplenishedNotice
				.Select (card => CardInfos[card].TransformView);

			// return observable of tuple (card transform, moveing positions for target as observable)
			return moveCardTransformObservable
				.Select (cardTransform => (cardTransform, CreateLinerMoves (cardTransform.position, target.TransformView.position, moveingFrame : 7)));
		}

		IObservable < IEnumerable < (Transform, IObservable<Vector3>) >> CreateDrawPlayObservable (PlayAreaInfo playAreaInfo) {
			var moveCardTransformsObservable = playAreaInfo.PlayAreaModel.PlayedNotice
				.Select (cards => cards.Select (card => CardInfos[card].TransformView).ToList ());

			return moveCardTransformsObservable
				.Select (cards => {
					var drawPlayCardBottomPosZ = (playAreaInfo.PlayAreaModel.CountPlayedCards () - cards.Count) * Card.thickness * Vector3.back;
					var moves = cards.Select ((card, i) => {
						var distance = 0.2f;
						var targetXY = distance * (-(cards.Count - 1) + 2 * i) * Vector3.left;
						var targetZ = drawPlayCardBottomPosZ + Card.thickness * i * Vector3.back;
						var currentPosition = playAreaInfo.TransformView.position;
						var nextPosZ = currentPosition + targetZ;
						var nextPos = nextPosZ + targetXY;
						return (cards[i], CreateLinerMoves (currentPosition, nextPosZ).Concat (CreateLinerMoves (nextPosZ, nextPos)));
					});
					return moves;
				});
		}

		IObservable < (Transform, IObservable<Vector3>) > CreateDrawFirstPutObservable (PlayAreaInfo playAreaInfo) {
			var moveCardTransformObservable = playAreaInfo.PlayAreaModel.FirstPutNotice
				.Select (card => CardInfos[card].TransformView);

			return moveCardTransformObservable
				.Select (cardTransform => (cardTransform, CreateLinerMoves (cardTransform.position, playAreaInfo.TransformView.position, 10)));
		}
	}
	public IEnumerator DrawShuffle () {

		// 設定項目
		var moveingFrame = 10;
		var moveVec = transform.right * 3f;

		var start = transform.position;
		var middle = start + moveVec;
		// middleまでmoveingFrameで動かす
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			transform.position = Vector3.Lerp (start, middle, (float) currentFrame / moveingFrame);
			yield return null;
		}
		// startまでmoveingFrameで動かす
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			transform.position = Vector3.Lerp (middle, start, (float) currentFrame / moveingFrame);
			yield return null;
		}
	}
}