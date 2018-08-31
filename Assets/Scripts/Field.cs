using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour {
	IList<Hand> hands;
	IList<Deck> decks;
	IList<PlayArea> playAreas;
	DiscardsBox discardsBox;

	void Awake () {
		hands = GetComponentsInChildren<Hand> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<PlayArea> ().ToList ();
		discardsBox = GetComponentInChildren<DiscardsBox> ();
	}

	public void SetUp () {
		void SetUpDecks () {
			foreach (var deck in decks) {
				deck.SetUp ();
			}
		}
		void ReplenishHands () {
			foreach (var hand in hands) {
				hand.Replenish ();
			}
		}
		SetUpDecks ();
		ReplenishHands ();
		ReplenishPlayCards ();
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var deck in decks) {
			deck.Delete ();
		}
		foreach (var hand in hands) {
			hand.Delete ();
		}
		foreach (var playArea in playAreas) {
			playArea.Delete ();
		}

		discardsBox.Delete ();
	}

	void ReplenishPlayCards () {
		foreach (var i in Enumerable.Range (0, hands.Count)) {
			playAreas[i].FirstPut(decks[i].TopDraw ());
		}
	}

	public void PlayCardsForHand (Hand playHand, PlayArea targetArea) {
		// カードプレイ&ハンド補充
		if (!targetArea.CanPlay (playHand.GetSelectedCards ())) return;
		targetArea.Play (playHand.RemoveSelectedCards ());
		playHand.Replenish ();

		StartCoroutine (targetArea.DrawPlayMoves ());
		StartCoroutine (playHand.DrawReplenishCards (7));

		// 次のカードがプレイできないときの処理
		void RemovePlayAreaCards () {
			foreach (var playArea in playAreas) {
				discardsBox.Add (playArea.RemoveAll ());
			}
		}
		bool CanNextPlay () {
			var existPlayableCards = playAreas.SelectMany (playArea =>
				hands.Select (hand =>
					playArea.ExistPlayableCards (hand)
				)).Any (judgement => judgement);
			return existPlayableCards;
		}

		if (CanNextPlay ()) return;
		Debug.Log ("CannotPlay!");
		RemovePlayAreaCards ();
		ReplenishPlayCards ();

		StartCoroutine (DrawNextPlacing ());
	}

	public IEnumerator DrawFirstCardPlacing () {
		// デッキシャッフル
		var shuffleDecks = decks.Select (deck => StartCoroutine (deck.DrawShuffle ()));
		yield return shuffleDecks.LastOrDefault (coroutine => coroutine != null);

		// 手札配置
		var placeHands = hands.Select (hand => StartCoroutine (hand.DrawReplenishCards (7)));
		yield return placeHands.LastOrDefault (coroutine => coroutine != null);

		// プレイエリア配置
		var placePlayAreas = playAreas.Select (playArea => StartCoroutine (playArea.DrawCardPlacing ()));
		yield return placePlayAreas.LastOrDefault (coroutine => coroutine != null);
	}

	IEnumerator DrawNextPlacing () {
		yield return new WaitForSeconds (2);
		yield return StartCoroutine (discardsBox.DrawRemovePlayAreaCards ());
		foreach (var playArea in playAreas) {
			StartCoroutine (playArea.DrawCardPlacing ());
		}
		yield return null;
	}
}