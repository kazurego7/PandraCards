using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldPlacer : MonoBehaviour {
	IList<CardPlacer> cardPlacers;
	IList<Deck> decks;
	IList<PlayArea> playAreas;
	DiscardsBox discardsBox;

	void Awake () {
		cardPlacers = GetComponentsInChildren<CardPlacer> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<PlayArea> ().ToList ();
		discardsBox = GetComponentInChildren<DiscardsBox> ();
	}

	public void Initialize () {
		foreach (var cardPlacer in cardPlacers) {
			cardPlacer.Initialize ();
		}
		PlayFristCards ();
	}

	void PlayFristCards () {
		foreach (var i in Enumerable.Range (0, cardPlacers.Count)) {
			var drawCard = decks[i].TopDraw ();
			if (drawCard != null) playAreas[i].PlayCards (new List<Card> () { drawCard });
		}
	}
	public IEnumerator DrawFirstCardPlacing () {
		// 手札配置
		var placeHands = cardPlacers.Select ((cardPlacer) => StartCoroutine (cardPlacer.DrawFirstCardPlacing ()));
		yield return placeHands.Last ((coroutine) => coroutine != null);

		// プレイエリア配置
		var placePlayAreas = playAreas.Select ((playArea) => StartCoroutine (playArea.DrawFirstCardPlacing ()));
		yield return placePlayAreas.Last ((coroutine) => coroutine != null);
	}

	IEnumerator DrawNextPlacing () {
		yield return new WaitForSeconds (2);
		yield return StartCoroutine (discardsBox.DrawRemovePlayAreaCards ());
		foreach (var playArea in playAreas) {
			StartCoroutine (playArea.DrawFirstCardPlacing ());
		}
		yield return null;
	}

	public void JudgeCanNextPlay () {
		void RemovePlayAreaCards () {
			discardsBox.Add (playAreas.Select (playArea => playArea?.RemovePlacedCards ())?.ToList ());
		}
		bool CanNextPlay () {
			var existPlayableCards = playAreas.SelectMany (playArea =>
				cardPlacers.Select (cardPlacer =>
					playArea.ExistPlayableCards (cardPlacer)
				)).Any (judgement => judgement);
			return existPlayableCards;
		}
		if (!CanNextPlay ()) {
			Debug.Log ("CannotPlay!");
			RemovePlayAreaCards ();
			PlayFristCards ();

			StartCoroutine (DrawNextPlacing ());
		}
	}

}