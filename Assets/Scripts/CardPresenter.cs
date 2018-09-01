using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardPresenter : MonoBehaviour {
	class CardInfo {
		public Transform transform;
	}

	class OneHandInfo {
		public OneHand oneHand;
		public Transform transform;
	}

	class DeckInfo {
		public Deck deck;
		public Transform transform;
	}

	class PlayAreaInfo {
		public PlayArea playArea;
		public Transform transform;
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

		void DrawReplinishe () {
			var replinishedFromToStream = Observable.Merge (
				OneHandInfos.Select (
					oneHandInfo => oneHandInfo.oneHand.ReplenishedNotice.Select (
						card => (from: CardInfos[card].transform, to: oneHandInfo.transform))));
			var changeTransformInfoStream = replinishedFromToStream.Select (
				fromTo => {
					var moveingFrame = 7;
					var changeVecs = Observable
						.IntervalFrame (moveingFrame)
						.Select (t => Vector3.Lerp (fromTo.from.position, fromTo.to.position + Card.thickness * Vector3.back, (float) t / moveingFrame));
					return (to: changeVecs, from: fromTo.from);
				});
			changeTransformInfoStream
				.Subscribe (info => info.to.Subscribe (changeVec => info.from.Translate (changeVec)));
		}
		var playedFromTo = Observable.Merge (
			PlayAreaInfos.Select (
				playAreaInfo => playAreaInfo.playArea.PlayedNotice.Select (
					cards => (cards.Select (card => CardInfos[card].transform), playAreaInfo.transform))));
		var firstPutFromTo = Observable.Merge (
			PlayAreaInfos.Select (
				playAreaInfo => playAreaInfo.playArea.FirstPutNotice.Select (
					card => (CardInfos[card].transform, playAreaInfo.transform))));
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

	public IEnumerator DrawPlayMoves () {
		var prevPlacedCardIndex = (PlayedCards.Count - 1) - 1; // prevPlacedCardIndex = placedCardIndex - 1
		var prevPlacedCardZ = prevPlacedCardIndex >= 0 ?
			PlayedCards[prevPlacedCardIndex].Last ().transform.position.z : // 注意!! 既にCardはPlayされ、placedCardsに格納されている
			0;
		var selectedCards = PlayedCards.Last ();
		foreach (var index in Enumerable.Range (0, selectedCards.Count)) {
			var leftmostDistance = 0.2f * (-(selectedCards.Count - 1) + 2 * index);
			var heightVector = (prevPlacedCardZ + -Card.thickness * (index + 1)) * Vector3.forward; // 注意!! 左手座標系(手前の方がマイナス)
			var movePosition = transform.position + leftmostDistance * Vector3.left + heightVector;
			StartCoroutine (selectedCards[index].DrawMove (selectedCards[index].transform.position + heightVector));
			StartCoroutine (selectedCards[index].DrawMove (movePosition, moveingFrame : 10));
		}
		yield return null;
	}
}