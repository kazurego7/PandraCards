using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlacer : MonoBehaviour {
	Deck deck;
	IList<HandPlace> handPlaces;

	void Awake () {
		deck = GetComponentInChildren<Deck> ();
		handPlaces = GetComponentsInChildren<HandPlace> ().ToList ();
	}

	public void Initialize () {
		deck.Initialize ();
		ReplenishHands (handPlaces);
	}

	public void ReplenishHands (IList<HandPlace> replenishHandPlaces) {
		// 手札
		foreach (var place in replenishHandPlaces) {
			place.PlacedCard = deck.TopDraw ();
		}
	}

	public IList<Card> GetHands(){
		return handPlaces.Select(handPlace => handPlace.PlacedCard).Where(card => card != null).ToList();
	}

	public IEnumerator DrawReplenishCards (int moveingFrame) {
		foreach (var place in handPlaces) {
			var card = place.PlacedCard;
			var movePosition = place.transform.position + Card.thickness * Vector3.back;
			card.DrawMove (movePosition, moveingFrame: moveingFrame);
			yield return card.Moveing;
		}
	}
	public IEnumerator DrawFirstCardPlacing () {
		yield return deck.DrawShuffle ();
		yield return DrawReplenishCards (10);
	}

}