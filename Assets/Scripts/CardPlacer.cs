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
			place.SetCard (deck.TopDraw ());
		}
	}

	public IEnumerator DrawReplenishCards (int moveingFrame) {
		foreach (var place in handPlaces) {
			var card = place.GetCard ();
			yield return StartCoroutine (card.DrawMove (place.transform.position, moveingFrame: moveingFrame));
		}
	}
	public IEnumerator DrawFirstCardPlacing () {
		yield return deck.DrawShuffle ();
		yield return DrawReplenishCards (10);
	}

}