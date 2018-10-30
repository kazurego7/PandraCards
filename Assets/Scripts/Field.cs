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
		// 処理
		foreach (var deck in decks) {
			deck.SetUp ();
		}
		// 描画
		var drawShuffle = decks.Select (deck => deck.DrawShulle ()).Merge ();

		// 手札配置
		// 処理
		foreach (var hand in hands) {
			hand.Replenish ();
		}
		// 描画
		var drawHandReplenish = hands.Select (hand => hand.DrawReplenish ()).Merge ();

		// プレイエリア配置
		// 処理
		foreach (var onePlayArea in playAreas) {
			onePlayArea.Replenish ();
		}
		// 描画
		var drawPlayAreaReplenish = playAreas.Select (playArea => playArea.DrawReplenish ()).Merge ();

		// 描画をdrawableに伝達
		drawable.SyncCommand.Execute (Observable.Concat (drawShuffle, drawHandReplenish, drawPlayAreaReplenish));
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
	public IObservable<Unit> PlayCardsForHand (Hand playHand, OnePlayArea targetArea) {

		// 
		if (!targetArea.CanPlay (playHand.GetSelectedCards ())) return Observable.ReturnUnit ();
		// カードプレイ
		targetArea.Play (playHand.RemoveSelectedCards ());
		var drawPlay = targetArea.DrawPlay ();
		// ハンド補充
		playHand.Replenish ();
		var drawHandReplenish = playHand.DrawReplenish ();

		// 次のカードがプレイできるように再配置する処理
		var canNextPlay = hands.Any (hand =>
			playAreas.Any (onePlayArea =>
				onePlayArea.CanNextPlay (hand)));

		// 次プレイできれば、再配置処理は必要ない
		if (canNextPlay) return drawPlay.Merge (drawHandReplenish);
		Debug.Log ("CannotPlay!");
		// プレイ済みのカードの廃棄処理
		foreach (var playArea in playAreas) {
			discardsBox.Store (playArea.RemoveAll ());
		}
		var drawRemoveForPlayArea = Observable.TimerFrame (120).AsUnitObservable ()
			.Concat (discardsBox.DrawRemoveForPlayArea ());
		// プレイエリアへの再配置
		foreach (var onePlayArea in playAreas) {
			onePlayArea.Replenish ();
		}
		var drawPlayAreaReplenish = playAreas.Select (playArea => playArea.DrawReplenish ()).Merge ();

		return drawPlay.Merge (drawHandReplenish).Concat (drawRemoveForPlayArea).Concat (drawPlayAreaReplenish);
	}
}