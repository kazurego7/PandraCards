using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Hand : MonoBehaviour {
	IList<OneHand> oneHands;
	Drawable drawable;

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

	public void SelectFrame (OneHand selecting) {
		var selectColor = selecting.PutCard?.MyColor;
		if (selectColor == null) {
			return;
		}

		var selecteds = oneHands.Where (oneHand => selecting != oneHand && oneHand.IsSelected);
		if (!selecteds.Any ()) {
			selecting.SelectFrame ();
			drawable.SyncCommand.Execute (selecting.DrawFrame ());
			return;
		}

		var selectedColor = selecteds
			.Select (selected => selected.PutCard.MyColor)
			.FirstOrDefault ();

		if (selectedColor == selectColor) {
			selecting.SelectFrame ();
			drawable.SyncCommand.Execute (selecting.DrawFrame ());
		} else {
			foreach (var selected in selecteds) {
				selected.DeselectFrame ();

			}
			selecting.SelectFrame ();

			foreach (var selected in selecteds) {
				drawable.SyncCommand.Execute (selected.DrawFrame ());
			}
			drawable.SyncCommand.Execute (selecting.DrawFrame ());
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

	public IObservable<Unit> DrawDeal () {
		return oneHands
			.Select (oneHand => oneHand.DrawDeal ())
			.Concat ();
	}

}