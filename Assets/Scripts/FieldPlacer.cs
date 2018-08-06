using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldPlacer : MonoBehaviour {
	IList<CardPlacer> cardPlacers;
	IList<Deck> decks;
	IList<PlayArea> playAreas;

	void Awake () {
		cardPlacers = GetComponentsInChildren<CardPlacer> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<PlayArea> ().ToList ();
	}

	public void Initialize () {
		foreach (var i in Enumerable.Range (0, cardPlacers.Count)) {
			cardPlacers[i].Initialize ();
			playAreas[i].PlayCard (decks[i].TopDraw ());
		}
	}

	public IEnumerator DrawFirstCardPlacing () {
		foreach (var cardPlacer in cardPlacers) {
			StartCoroutine (cardPlacer.DrawFirstCardPlacing ());
		}
		yield return null;
	}

}