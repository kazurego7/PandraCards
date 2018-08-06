using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlacer : MonoBehaviour {
	IDeck deck;
	IList<HandPlace> handPlaces;

	void Awake () {
		deck = GetComponentInChildren<Deck> ();
		handPlaces = GetComponentsInChildren<HandPlace> ().ToList ();
	}
	public void Initialize () {
		deck.Initialize ();
		ReplenishCards ();
	}

	void ReplenishCards () {
		// 手札
		foreach (var place in handPlaces) {
			place.SetCard(deck.TopDraw ());
		}
	}
	public IEnumerator DrawFirstCardPlacing () {
		IEnumerator DrawReplenishCards () {
			foreach (var place in handPlaces) {
				var card = place.GetCard().GetComponent<Card>();
				yield return StartCoroutine(card.DrawMove(place.transform.position));
			}
		}
		yield return deck.DrawShuffle ();
		yield return DrawReplenishCards ();
	}

}