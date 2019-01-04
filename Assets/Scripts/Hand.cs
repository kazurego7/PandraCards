using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Hand : MonoBehaviour {
	IList<OneHand> oneHands;
	void Awake () {
		oneHands = GetComponentsInChildren<OneHand> ().ToList ();
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var hand in oneHands) {
			Destroy (hand.Remove ()?.gameObject);
		}
	}

	public void Deal (Deck yourDeck) {
		foreach (var oneHand in oneHands) {
			if (oneHand.PutCard == null) {
				var drawCard = yourDeck.TopDraw ();
				// Debug.Log(drawCard);
				oneHand.Deal (drawCard);
			};
		}
	}

	public IList<Card> GetAllCards () {
		return oneHands
			.Select (oneHand => oneHand.PutCard) ?
			.Where (card => card != null)
			.ToList ();
	}

	public IList<Card> GetSelectedCards () {
		return oneHands
			.Where (oneHand => oneHand.IsSelected)
			.Select (selected => selected.PutCard)
			.ToList ();
	}

	public IList<Card> RemoveSelectedCards () {
		return oneHands
			.Where (oneHand => oneHand.IsSelected)
			.Select (selected => selected.Remove ())
			.ToList ();
	}

	public void DecelectFrame () {
		foreach (var onehand in oneHands) {
			onehand.DeselectFrame ();
		}
	}

	public IObservable<Unit> DrawDeal () {
		return oneHands
			.Select (oneHand => oneHand.DrawDeal ())
			.Concat ();
	}

	public IObservable<Unit> DrawFrame () {
		return oneHands.Select (onehand => onehand.DrawFrame ()).Merge ();
	}
}