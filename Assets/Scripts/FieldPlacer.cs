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

	public void SetUp () {
		void SetUpDecks () {
			foreach (var deck in decks) {
				deck.SetUp ();
			}
		}
		void ReplenishHands () {
			foreach (var placer in cardPlacers) {
				placer.ReplenishHands ();
			}
		}
		SetUpDecks ();
		ReplenishHands ();
		ReplenishPlayCards ();
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var deck in decks) {
			deck.Delete ();
		}
		foreach (var cardPlacer in cardPlacers) {
			cardPlacer.Delete ();
		}
		foreach (var playArea in playAreas) {
			playArea.Delete();
		}

		discardsBox.Delete();
	}

	void ReplenishPlayCards () {
		foreach (var i in Enumerable.Range (0, cardPlacers.Count)) {
			var drawCard = decks[i].TopDraw ();
			if (drawCard != null) playAreas[i].PlayCards (new List<Card> () { drawCard });
		}
	}
	public IEnumerator DrawFirstCardPlacing () {
		// デッキシャッフル
		var shuffleDecks = decks.Select (deck => StartCoroutine (deck.DrawShuffle ()));
		yield return shuffleDecks.LastOrDefault (coroutine => coroutine != null);

		// 手札配置
		var placeHands = cardPlacers.Select (cardPlacer => StartCoroutine (cardPlacer.DrawReplenishCards (7)));
		yield return placeHands.LastOrDefault (coroutine => coroutine != null);

		// プレイエリア配置
		var placePlayAreas = playAreas.Select (playArea => StartCoroutine (playArea.DrawCardPlacing ()));
		yield return placePlayAreas.LastOrDefault (coroutine => coroutine != null);
	}

	IEnumerator DrawNextPlacing () {
		yield return new WaitForSeconds (2);
		yield return StartCoroutine (discardsBox.DrawRemovePlayAreaCards ());
		foreach (var playArea in playAreas) {
			StartCoroutine (playArea.DrawCardPlacing ());
		}
		yield return null;
	}

	void JudgeCanNextPlay () {
		void RemovePlayAreaCards () {
			foreach (var playArea in playAreas)
			{
				discardsBox.Add(playArea.RemovePlacedCards());
			}
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
			RemovePlayAreaCards();
			ReplenishPlayCards ();

			StartCoroutine (DrawNextPlacing ());
		}
	}

	public void PlayCardsForHands (CardPlacer cardPlacer, PlayArea playArea) {
		var selectedCards = cardPlacer.GetSelectedHands ();
		if (playArea.CanPlayCards (selectedCards)) {
			var removedCards = cardPlacer.RemoveSelectedHands ();
			playArea.PlayCards (removedCards);
			cardPlacer.ReplenishHands ();

			StartCoroutine (playArea.DrawCardPlayMoves ());
			StartCoroutine (cardPlacer.DrawReplenishCards (7));

			JudgeCanNextPlay ();
		}
	}

}