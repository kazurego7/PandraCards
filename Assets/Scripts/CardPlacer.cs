using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPlacer : MonoBehaviour {
	IDeck deck;
	IList<Transform> places;

	// Use this for initialization
	void Start () {
		deck = GetComponentInChildren<Deck> ();
		deck.InitDeck ();
		InitPlaces ();
		StartCoroutine (DrawFirstCardPlaced ());
	}

	void InitPlaces () {
		var playArea = transform.Find ("PlayArea").transform;
		places = playArea.GetComponentsInChildren<Transform> ().OrderBy (t => t.name).ToList ();
	}

	IEnumerator DrawFirstCardPlaced () {
		yield return deck.DrawShuffle ();
		DrawReplenishCard ();
		yield return null;
	}

	void DrawReplenishCard () {
		foreach (var place in places) {
			var card = deck.TopDraw ();
			card.transform.position = place.position;
		}
	}
}