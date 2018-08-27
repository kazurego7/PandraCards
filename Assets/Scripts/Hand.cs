using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour {
	Deck deck;
	IList<HandPlace> handPlaces;

	void Awake () {
		deck = transform.parent.GetComponentInChildren<Deck> ();
		handPlaces = GetComponentsInChildren<HandPlace> ().ToList ();
	}

    public void Delete() {
		StopAllCoroutines ();
		foreach (var hand in handPlaces)
		{
			Destroy(hand.RemoveCard()?.gameObject);
		}
	}

	public void ReplenishHands (){
		foreach (var place in handPlaces){
			if (place.PutCard == null) place.PutCard = deck.TopDraw();
		}
	}

	public IList<Card> GetCardsAll(){
		return handPlaces.Select(handPlace => handPlace.PutCard)?.Where(card => card != null).ToList();
	}

	public IList<Card> GetSelectedCards() {
		return handPlaces.Where(handPlace => handPlace.IsSelcted).Select(selected => selected.PutCard).ToList();
	}

	public IList<Card> RemoveSelectedHands(){
		return handPlaces.Where(handPlace => handPlace.IsSelcted).Select(selected => selected.RemoveCard()).ToList();
	}
	public IEnumerator DrawReplenishCards (int moveingFrame) {
		foreach (var place in handPlaces) {
			yield return StartCoroutine(place.DrawReplaceCards(moveingFrame));
		}
	}

}