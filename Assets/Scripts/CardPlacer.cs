using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlacer : MonoBehaviour {
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
			if (place.PlacedCard == null) place.PlacedCard = deck.TopDraw();
		}
	}

	public IList<Card> GetHandsAll(){
		return handPlaces.Select(handPlace => handPlace.PlacedCard)?.Where(card => card != null).ToList();
	}

	public IList<Card> GetSelectedHands() {
		return handPlaces.Where(handPlace => handPlace.IsSelcted).Select(selected => selected.PlacedCard).ToList();
	}

	public IList<Card> RemoveSelectedHands(){
		return handPlaces.Where(handPlace => handPlace.IsSelcted).Select(selected => selected.RemoveCard()).ToList();
	}
	public IEnumerator DrawReplenishCards (int moveingFrame) {
		foreach (var place in handPlaces) {
			var card = place.PlacedCard;
			var movePosition = place.transform.position + Card.thickness * Vector3.back;
			
			if(card != null) yield return StartCoroutine(card.DrawMove (movePosition, moveingFrame: moveingFrame));
		}
	}

}