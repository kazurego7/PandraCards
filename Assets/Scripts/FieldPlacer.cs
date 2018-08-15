using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldPlacer : MonoBehaviour {
	IList<CardPlacer> cardPlacers;
	IList<Deck> decks;
	IList<PlayArea> playAreas;

	IList<IList<IList<Card>> > discardsBox;

	void Awake () {
		cardPlacers = GetComponentsInChildren<CardPlacer> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<PlayArea> ().ToList ();
		discardsBox = new List<IList<IList<Card>> > ();
	}

	public void Initialize () {
		foreach (var cardPlacer in cardPlacers) {
			cardPlacer.Initialize ();
		}
		PlayFristCards ();
	}

	void PlayFristCards () {
		foreach (var i in Enumerable.Range (0, cardPlacers.Count)) {
			playAreas[i].PlayCards (new List<Card> () { decks[i].TopDraw () });
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
		IEnumerator DrawRemovePlayAreaCards () {
			var linerDiscards = discardsBox.SelectMany (x => x).SelectMany (x => x);
			Debug.Log(linerDiscards.Count());
			foreach (var discard in linerDiscards) {
				yield return discard.Moveing;
				Debug.Log(discard.Moveing);
				Destroy (discard.gameObject);
			}
			yield return null;
		}
		yield return new WaitForSeconds(2);
		yield return StartCoroutine (DrawRemovePlayAreaCards ());
		foreach (var playArea in playAreas) {
			StartCoroutine (playArea.DrawFirstCardPlacing ());
		}
		yield return null;
	}

	public void JudgeCanNextPlay () {
		void RemovePlayAreaCards () {
			foreach (var i in Enumerable.Range (0, playAreas.Count)) {
				discardsBox.Add (playAreas[i].RemovePlacedCards ());
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
			RemovePlayAreaCards ();
			PlayFristCards ();

			StartCoroutine (DrawNextPlacing ());
		}
	}

}