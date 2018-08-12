using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldPlacer : MonoBehaviour {
	IList<CardPlacer> cardPlacers;
	IList<Deck> decks;
	IList<PlayArea> playAreas;

	IList<IList<IList<Card>>> discardsBox;

	void Awake () {
		cardPlacers = GetComponentsInChildren<CardPlacer> ().ToList ();
		decks = GetComponentsInChildren<Deck> ().ToList ();
		playAreas = GetComponentsInChildren<PlayArea> ().ToList ();
	}

	public void Initialize () {
		foreach (var cardPlacer in cardPlacers) {
			cardPlacer.Initialize ();
		}
		PlaceFristCard();
	}

	public void PlaceFristCard(){
		foreach (var i in Enumerable.Range (0, cardPlacers.Count)) {
			playAreas[i].PlayCards (new List<Card>() {decks[i].TopDraw ()});
		}
	}
	public IEnumerator DrawFirstCardPlacing () {
		// 手札配置
		var placeHands = cardPlacers.Select((cardPlacer) => StartCoroutine(cardPlacer.DrawFirstCardPlacing()));
		yield return placeHands.Last((coroutine)=> coroutine != null);

		// プレイエリア配置
		var placePlayAreas = playAreas.Select((playArea) => StartCoroutine(playArea.DrawFirstCardPlacing()));
		yield return placePlayAreas.Last((coroutine)=> coroutine != null);
	}

	public void JudgeCanNextPlay(){
		void RemovePlacedCards() {
			playAreas.Zip(discardsBox, (playArea,discard) => discard = playArea.RemovePlacedCards());
		}

		IEnumerator DrawRemovePlacedCards() {
			var linerDiscards = discardsBox.SelectMany(x => x).SelectMany(x => x);
			foreach (var discard in linerDiscards)
			{
				Destroy(discard.gameObject);
			}
			yield return null;
		}

		bool CanNextPlay() {
			
			return true;
		}
		if (!CanNextPlay()){
			RemovePlacedCards();
			PlaceFristCard();

			foreach (var playArea in playAreas)
			{
				StartCoroutine(DrawRemovePlacedCards());
				StartCoroutine(playArea.DrawFirstCardPlacing());
			}
		}
	}

}