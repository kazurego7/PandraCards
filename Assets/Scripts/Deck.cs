using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour {
	IList<Card> cards;

	DeckReciper deckReciper;

	public void Awake() {
		deckReciper = GetComponent<DeckReciper>();
	}

	public void SetUp () {
		this.cards = deckReciper.CreateDeck();

		Shuffle ();
		AdjustCardHeights ();
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var card in cards) {
			Destroy (card.gameObject);
		}
	}
	void AdjustCardHeights () {
		// デッキのそれぞれのカードの高さを厚みによって調節
		foreach (var index in Enumerable.Range (0, cards.Count)) {
			var heightAjustedPosition = cards[index].transform.position + index * Card.thickness * Vector3.back;
			var card = cards[index];
			card.transform.Rotate(heightAjustedPosition);
		}
	}

	public Card TopDraw () {
			var top = cards.LastOrDefault ();
			if (cards.Count > 0) cards.RemoveAt (cards.Count - 1);
			return top;
	}

	/*********************************************
	/*	シャッフル系
	/*********************************************/
	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		cards = cards.OrderBy (_ => Guid.NewGuid ()).ToList ();
		return;
	}

	public IEnumerator DrawShuffle () {
		// 各カードが動き始めるのを何秒遅延するか
		var startDelaySecond = 0.01f;
		Coroutine lastCoroutine = null;
		foreach (var card in cards) {
			lastCoroutine = StartCoroutine (card.DrawShuffle ());
			yield return new WaitForSeconds (startDelaySecond);
		}

		// 最後のコルーチンだけ返すことで、順序処理が行える
		yield return lastCoroutine;
	}
}