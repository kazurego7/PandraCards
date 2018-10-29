using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Hand : MonoBehaviour {
	Deck deck;
	IList<OneHand> oneHands;

	void Awake () {
		deck = transform.parent.GetComponentInChildren<Deck> ();
		oneHands = GetComponentsInChildren<OneHand> ().ToList ();
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var hand in oneHands) {
			Destroy (hand.RemoveCard ()?.gameObject);
		}
	}

	public void Replenish () {
		foreach (var oneHand in oneHands) {
			if (oneHand.PutCard == null) {
				var drawCard = deck.TopDraw ();
				// Debug.Log(drawCard);
				oneHand.Replenish (drawCard);
			};
		}
	}

	public IList<Card> GetAllCards () {
		return oneHands.Select (handPlace => handPlace.PutCard)?.Where (card => card != null).ToList ();
	}

	public IList<Card> GetSelectedCards () {
		return oneHands.Where (handPlace => handPlace.IsSelcted.Value).Select (selected => selected.PutCard).ToList ();
	}

	public IList<Card> RemoveSelectedCards () {
		return oneHands.Where (handPlace => handPlace.IsSelcted.Value).Select (selected => selected.RemoveCard ()).ToList ();
	}

	public IObservable<Unit> DrawReplenish () {
		return oneHands
			.Select (oneHand => oneHand.DrawReplenish ())
			.Concat ();
	}

}