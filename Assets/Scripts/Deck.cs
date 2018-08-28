using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class Deck : MonoBehaviour {
	public ReactiveCollection<Card> Cards {
		get;
		private set;
	}

	DeckReciper deckReciper;

	public void Awake() {
		deckReciper = GetComponent<DeckReciper>();
	}

	public void SetUp () {
		Cards = new ReactiveCollection<Card>(deckReciper.CreateDeck());

		Shuffle ();
		StartCoroutine(DrawAdjustCardHeights ());
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var card in Cards) {
			Destroy (card.gameObject);
		}
	}

	public Card TopDraw () {
			var top = Cards.LastOrDefault ();
			if (Cards.Count > 0) Cards.RemoveAt (Cards.Count - 1);
			return top;
	}

	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		Cards = Cards.OrderBy (_ => Guid.NewGuid ()).ToReactiveCollection();
		return;
	}

	public IEnumerator DrawShuffle () {
		// 各カードが動き始めるのを何秒遅延するか
		var startDelaySecond = 0.01f;
		Coroutine lastCoroutine = null;
		foreach (var card in Cards) {
			lastCoroutine = StartCoroutine (card.DrawShuffle ());
			yield return new WaitForSeconds (startDelaySecond);
		}

		// 最後のコルーチンだけ返すことで、順序処理が行える?
		yield return lastCoroutine;
	}

	IEnumerator DrawAdjustCardHeights () {
		// デッキのそれぞれのカードの高さを厚みによって調節
		foreach (var index in Enumerable.Range (0, Cards.Count)) {
			var heightAjustedPosition = Cards[index].transform.position + index * Card.thickness * Vector3.back;
			var card = Cards[index];
			card.transform.Rotate(heightAjustedPosition);
		}
		yield return null;
	}
}