﻿using System;
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
		ReplenishCards();
		StartCoroutine (DrawFirstCardPlacing ());
	}

	void InitPlaces () {
		var placer = transform.Find ("HandPlacer").transform;
		places = placer.GetComponentsInChildren<Transform> ().Where (t => t != placer).OrderBy (t => t.name).ToList ();
	}

	void ReplenishCards() {
		foreach (var place in places) {
		var card = deck.TopDraw ();
			card.transform.SetParent (place);
		}
	}
	IEnumerator DrawFirstCardPlacing () {
		yield return deck.DrawShuffle ();
		yield return DrawReplenishCards ();
	}

	IEnumerator DrawReplenishCards () {
		foreach (var place in places) {
			yield return StartCoroutine (DrawReplenishOne (place.GetChild(0).transform, place.position));
		}
	}

	IEnumerator DrawReplenishOne (Transform card, Vector3 target) {
		var moveingFrame = 10;
		var start = card.position;
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			card.position = Vector3.Lerp (start, target, (float) currentFrame / moveingFrame);
			yield return null;
		}
	}
}