using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardPresenter : MonoBehaviour {
	class CardInfo {
		public Card card;
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
		var replinishedFromToPair = Observable.Merge (
			OneHandInfos.Select (
				oneHandInfo => oneHandInfo.oneHand.ReplenishedNotice.Select (
					card => (CardInfos[card].transform, oneHandInfo.transform))));
		var playedFromToPair = Observable.Merge (
			PlayAreaInfos.Select (
				playAreaInfo => playAreaInfo.playArea.PlayedNotice.Select (
					cards => (cards.Select(card => CardInfos[card].transform), playAreaInfo.transform))));
		var firstPutFromToPair = Observable.Merge (
			PlayAreaInfos.Select (
				playAreaInfo => playAreaInfo.playArea.FirstPutNotice.Select (
					card => (CardInfos[card].transform, playAreaInfo.transform))));
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator DrawMove (Vector3 target, int moveingFrame = 1) {
		var start = transform.position;
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			transform.position = Vector3.Lerp (start, target, (float) currentFrame / moveingFrame);
			yield return null;
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