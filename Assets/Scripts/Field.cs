using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour {
	IList<Hand> hands;
	IList<Deck> decks;
	IList<OnePlayArea> playAreas;
	DiscardsBox discardsBox;

	void Awake () {
		hands = GetComponentsInChildren<Hand> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<OnePlayArea> ().ToList ();
		discardsBox = GetComponentInChildren<DiscardsBox> ();
	}

	public void SetUp () {
		foreach (var deck in decks) {
			deck.SetUp ();
		}
		foreach (var hand in hands) {
			hand.Replenish ();
		}
		foreach (var onePlayArea in playAreas) {
			onePlayArea.Replenish ();
		}
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
	public void PlayCardsForHand (Hand playHand, OnePlayArea targetArea) {

		// カードプレイ&ハンド補充
		if (!targetArea.CanPlay (playHand.GetSelectedCards ())) return;
		targetArea.Play (playHand.RemoveSelectedCards ());
		StartCoroutine (targetArea.DrawPlayMoves ());
		playHand.Replenish ();
		StartCoroutine (playHand.DrawReplenishCards (7));

		// 次のカードがプレイできるように再配置する処理
		var canNextPlay = hands.Any (hand =>
			playAreas.Any (onePlayArea =>
				onePlayArea.CanNextPlay (hand)));

		if (canNextPlay) return; // 次プレイできれば、再配置処理は必要ない
		Debug.Log ("CannotPlay!");
		foreach (var playArea in playAreas) { // プレイ済みのカードの廃棄処理
			discardsBox.Store (playArea.RemoveAll ());
		}
		foreach (var onePlayArea in playAreas) { // プレイエリアへの再配置
			onePlayArea.Replenish ();
		}

		StartCoroutine (DrawNextPlacing ());
	}

	public IEnumerator DrawFirstCardPlacing () {
		// デッキシャッフル
		var shuffleDecks = decks.Select (deck => StartCoroutine (deck.ShuffleDraw ()));
		yield return shuffleDecks.LastOrDefault (coroutine => coroutine != null);

		// 手札配置
		var replenishHands = hands.Select (hand => StartCoroutine (hand.DrawReplenishCards (7)));
		yield return replenishHands.LastOrDefault (coroutine => coroutine != null);

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