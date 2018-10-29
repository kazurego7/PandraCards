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

	public IReactiveProperty<IList<Card>> ShuffledNotice {
		get;
		private set;
	} = new ReactiveProperty<IList<Card>> ();

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
		var top = Cards.Last ();
		Cards.RemoveAt (Cards.Count - 1); // remove Last
		return top;
	}

	public Card GetNthCardFromTop (int n) {
		if (n > Cards.Count || n < 1) return null;
		return Cards[n - 1];
	}

	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		Cards = Cards.OrderBy (_ => Guid.NewGuid ()).ToList ();
		ShuffledNotice.Value = Cards;
		return;
	}

	public IObservable<Unit> DrawShulle () {
		// 各カードが動き始めるのを何F遅延するか
		var startDelay = 1;
		return Observable
			.TimerFrame (0, startDelay).Take (Cards.Count)
			.SelectMany (count => Cards[(int) count].DrawShuffle ());
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