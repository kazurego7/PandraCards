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
				.TimerFrame (0, 1)
				.TakeWhile (t => t <= moveingFrame)
				.Select (t => Vector3.Lerp (source, target, (float) t / moveingFrame));
		}
		IObservable < IObservable < (Transform, Vector3) >> CreateDrawShuffleObservable (DeckInfo deckInfo) {
			return deckInfo.DeckModel.ShuffledNotice
				.Select (_ => {
					var deckCardCount = deckInfo.DeckModel.CountCard ();

					// 各カードの動き初めをdelayTimeだけずらし、各々の動きのストリームを作って最後にマージしている。
					var delayFrame = 1;
					return Observable
						.TimerFrame (0, delayFrame)
						.TakeWhile (t => t <= deckCardCount)
						.SelectMany (tCount => {
							// 各カード1枚ごとの処理
							var moveVec = transform.right * 3f;
							var start = deckInfo.TransformView.position;
							var middle = start + moveVec;
							var card = deckInfo.DeckModel.GetNthCardFromTop ((int) tCount);
							var cardTransform = CardInfos[card].TransformView;

							return CreateLinerMoves (start, middle, moveingFrame : 10)
								.Concat (CreateLinerMoves (middle, start, moveingFrame : 10))
								.Select (moveing => (cardTransform, moveing));
						});
				});
		}
		IObservable < IObservable < (Transform, Vector3) >> CreateDrawReplenisheObservable (OneHandInfo oneHandInfo) {
			// Noticeを集めて、動くCardのTransformに変換する
			return oneHandInfo.OneHandModel.ReplenishedNotice
				.Select (card => {
					var cardTransform = CardInfos[card].TransformView;
					return CreateLinerMoves (cardTransform.position, oneHandInfo.TransformView.position, moveingFrame : 7)
						.Select (moveing => (cardTransform, moveing));
				});
		}
		IObservable < IObservable < (Transform, Vector3) >> CreateDrawPlayObservable (PlayAreaInfo playAreaInfo) {
			return playAreaInfo.PlayAreaModel.PlayedNotice
				.Select (cards => {
					var cardTransforms = cards.Select (card => CardInfos[card].TransformView).ToList ();
					var drawPlayCardBottomPosZ = (playAreaInfo.PlayAreaModel.CountPlayedCards () - cardTransforms.Count) * Card.thickness * Vector3.back;
					var playCardMoves = cardTransforms.Select ((card, i) => {
						var distance = 0.2f;
						var targetXY = distance * (-(cardTransforms.Count - 1) + 2 * i) * Vector3.left;
						var targetZ = drawPlayCardBottomPosZ + Card.thickness * i * Vector3.back;
						var currentPosition = playAreaInfo.TransformView.position;
						var nextPosZ = currentPosition + targetZ;
						var nextPos = nextPosZ + targetXY;
						var onePlayCardMove = CreateLinerMoves (currentPosition, nextPosZ)
							.Concat (CreateLinerMoves (nextPosZ, nextPos))
							.Select (moveing => (target: cardTransforms[i], moveing));
						return onePlayCardMove;
					});
					return Observable.Merge (playCardMoves);
				});
		}
		IObservable < IObservable < (Transform, Vector3) >> CreateDrawFirstPutObservable (PlayAreaInfo playAreaInfo) {
			return playAreaInfo.PlayAreaModel.FirstPutNotice
				.Select (card => {
					var cardTransform = CardInfos[card].TransformView;
					return CreateLinerMoves (cardTransform.position, playAreaInfo.TransformView.position, 10)
						.Select (moveing => (cardTransform, moveing));
				});
		}

		var drawShuffleStream = DeckInfos
			.Select (info => CreateDrawShuffleObservable (info))
			.Merge ();
		var drawReplenishStream = OneHandInfos
			.Select (info => CreateDrawReplenisheObservable (info))
			.Merge ();
		var drawPlayStream = PlayAreaInfos
			.Select (info => CreateDrawPlayObservable (info))
			.Merge ();
		var drawFirstPutStream = PlayAreaInfos
			.Select (info => CreateDrawFirstPutObservable (info))
			.Merge ();

		var cardMoveStream = Observable
			.Merge (drawShuffleStream, drawReplenishStream, drawPlayStream, drawFirstPutStream)
			.Concat ()
			.Subscribe (cardMoveInfo => {
				var (obj, target) = cardMoveInfo;
				obj.Translate (target);
			});

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