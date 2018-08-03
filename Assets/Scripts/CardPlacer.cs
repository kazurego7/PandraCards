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
			IEnumerator DrawReplenishOne (Transform card, Vector3 target) {
				var moveingFrame = 10;
				var start = card.position;
				foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
					card.position = Vector3.Lerp (start, target, (float) currentFrame / moveingFrame);
					yield return null;
				}
			}
			foreach (var place in handPlaces) {
				yield return StartCoroutine (DrawReplenishOne (place.GetCard(), place.transform.position));
			}
		}
		yield return deck.DrawShuffle ();
		yield return DrawReplenishCards ();
	}

}