using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardPresenter : MonoBehaviour {
	class CardInfo {
		public Transform TransformView {
			get;
			private set;
		}
	}

	class OneHandInfo {
		public OneHand OneHandModel {
			get;
			private set;
		}
		public Transform TransformView {
			get;
			private set;
		}
	}

	class DeckInfo {
		public Deck DeckModel {
			get;
			private set;
		}
		public Transform TransformView {
			get;
			private set;
		}
	}

	class PlayAreaInfo {
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
		IObservable<Vector3> CreateLinerMoves (Vector3 from, Vector3 to, int moveingFrame = 1) {
			return Observable
				.IntervalFrame (moveingFrame)
				.Select (t => Vector3.Lerp (from, to + Card.thickness * Vector3.back, (float) t / moveingFrame));
		}

		IObservable < (Transform, IObservable<Vector3>) > GetDrawReplinisheObservable () {
			// Noticeを集めて、動くCardのTransform (from)と、動いた先のHandのTransform (to)に変換する
			var moveFromToObservables = OneHandInfos
				.Select (oneHandInfo => oneHandInfo.OneHandModel.ReplenishedNotice
					.Select (card => (source: CardInfos[card].TransformView, target: oneHandInfo.TransformView)));

			// (from, to)から、どのように動くか
			return Observable
				.Merge (moveFromToObservables)
				.Select (t => {
					var (source, target) = t; // 関数の仮引数では、タプルが分解できん（怒）
					return (source, CreateLinerMoves(source.position, target.position, moveingFrame: 7));
				});
		}
		IObservable < (IEnumerable<Transform>, IObservable<Vector3>) > GetDrawPlayObservable () {
			var moveFromToObservables = PlayAreaInfos
			.Select(playAreaInfo => playAreaInfo.PlayAreaModel.PlayedNotice
			.Select(cards => (sources: cards.Select(card => CardInfos[card].TransformView), target: playAreaInfo.TransformView)));

			return Observable
				.Merge(moveFromToObservables)
				.Select(t => {
					var (sources, target) = t;
					return (sources, )		// ***************ここから
				});
		}

		var playFromTo = Observable.Merge (
			PlayAreaInfos.Select (
				playAreaInfo => playAreaInfo.PlayAreaModel.PlayedNotice.Select (
					cards => (cards.Select (card => CardInfos[card].TransformView), playAreaInfo.TransformView))));
		var firstPutFromTo = Observable.Merge(
            PlayAreaInfos.Select(
				playAreaInfo => playAreaInfo.PlayAreaModel.FirstPutNotice.Select(
					card => ((TransformView: CardInfos[card].TransformView, transform:playAreaInfo.TransformView)))));
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