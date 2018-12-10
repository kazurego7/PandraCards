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
	Drawable drawable;

	void Awake () {
		hands = GetComponentsInChildren<Hand> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<OnePlayArea> ().ToList ();
		discardsBox = GetComponentInChildren<DiscardsBox> ();
		drawable = GetComponentInParent<Drawable> ();
	}

	public void SetUp () {

		// デッキシャッフル
		foreach (var deck in decks) {
			deck.SetUp ();
		}
		var drawShuffle = decks.Select (deck => deck.DrawShulle ()).Merge ();

		// 手札配置
		foreach (var hand in hands) {
			hand.Deal (decks[0]);
		}
		var drawHandDeal = hands.Select (hand => hand.DrawDeal ()).Merge ();

		// プレイエリア配置
		foreach (var playArea in playAreas) {
			playArea.Deal (decks[0].TopDraw ());
		}
		var drawPlayAreaDeal = playAreas.Select (playArea => playArea.DrawDeal ()).Merge ();

		// 描画をdrawableに伝達
		drawable.SyncCommand.Execute (Observable.Concat (drawShuffle));
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
	public void PlayForHand (Hand playHand, OnePlayArea targetArea) {

		if (!targetArea.CanPlay (playHand.GetSelectedCards ())) return;
		// カードプレイ
		targetArea.Play (playHand.RemoveSelectedCards ());
		var drawPlay = targetArea.DrawPlay ();
		// ハンド補充
		playHand.Deal (decks[0]);
		var drawHandDeal = playHand.DrawDeal ();

		drawable.SyncCommand.Execute (drawPlay.Merge (drawHandDeal));

		// 次のカードがプレイできるように再配置する処理
		var canNextPlay = hands.Any (hand =>
			playAreas.Any (onePlayArea =>
				onePlayArea.CanNextPlay (hand)));

		// 次プレイできれば、再配置処理は必要ない
		if (canNextPlay) return;

		Debug.Log ("CannotPlay!");
		// プレイ済みのカードの廃棄処理
		foreach (var playArea in playAreas) {
			discardsBox.Store (playArea.RemoveAll ());
		}
		var drawRemoveForPlayArea = Observable.TimerFrame (120).AsUnitObservable ()
			.Concat (discardsBox.DrawRemoveForPlayArea ());
		// プレイエリアへの再配置
		foreach (var onePlayArea in playAreas) {
			onePlayArea.Deal (decks[0].TopDraw ());
		}
		var drawPlayAreaDeal = playAreas.Select (playArea => playArea.DrawDeal ()).Merge ();

		drawable.SyncCommand.Execute (drawRemoveForPlayArea.Concat (drawPlayAreaDeal));
		return;
	}
}