using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Deck : MonoBehaviour {
	public IList<Card> Cards {
		get;
		private set;
	}

	DeckReciper deckReciper;

	public IReactiveProperty<ShuffleMsg> ShuffledNotice {
		get;
		private set;
	} = new ReactiveProperty<ShuffleMsg> ();

	public void Awake () {
		deckReciper = GetComponent<DeckReciper> ();
	}

	public void SetUp () {
		Cards = deckReciper.CreateDeck ();

		Shuffle ();
		StartCoroutine (DrawAdjustCardHeights ());
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var card in Cards) {
			Destroy (card.gameObject);
		}
	}

	public int CountCard () {
		return Cards.Count;
	}

	public Card TopDraw () {
		if (Cards.Count < 1) return null;
		Cards.RemoveAt (Cards.Count - 1);
		return Cards.Last ();
	}

	public Card GetNthCardFromTop (int n) {
		if (n > Cards.Count || n < 1) return null;
		return Cards[n - 1];
	}

	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		Cards = Cards.OrderBy (_ => Guid.NewGuid ()).ToList ();
		ShuffledNotice.Value = new ShuffleMsg (Cards);
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
			card.transform.Rotate (heightAjustedPosition);
		}
		yield return null;
	}
}