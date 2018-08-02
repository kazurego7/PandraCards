using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlacer : MonoBehaviour {
	IDeck deck;
	IList<Transform> places;

	[SerializeField] Transform handPlacer;

	void Awake () {
		deck = GetComponentInChildren<Deck> ();
		places = handPlacer.GetComponentsInChildren<Transform> ().Where (t => t != handPlacer).OrderBy (t => t.name).ToList ();
	}
	public void Initialize () {
		deck.Initialize ();
		ReplenishCards ();
	}

	void ReplenishCards () {
		// 手札
		foreach (var place in places) {
			var card = deck.TopDraw ();
			card.transform.SetParent (place);
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
			foreach (var place in places) {
				yield return StartCoroutine (DrawReplenishOne (place.GetChild (0).transform, place.position));
			}
		}
		yield return deck.DrawShuffle ();
		yield return DrawReplenishCards ();
	}

}