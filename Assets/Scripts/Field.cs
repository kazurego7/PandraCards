using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
		StartCoroutine (playHand.DrawReplenishCards ());

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

	public IObservable<Unit> DrawFirstCardPlacing () {
		// デッキシャッフル
		var shuffleDecks = decks.Select (deck => Observable.FromCoroutine (deck.ShuffleDraw)).Merge ();

		// 手札配置
		var replenishHands = hands.Select (hand => Observable.FromCoroutine (hand.DrawReplenishCards)).Merge ();

		// プレイエリア配置
		var placePlayAreas = playAreas.Select (playArea => Observable.FromCoroutine (playArea.DrawCardPlacing)).Merge ();
		return Observable.Concat (shuffleDecks, replenishHands, placePlayAreas);
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