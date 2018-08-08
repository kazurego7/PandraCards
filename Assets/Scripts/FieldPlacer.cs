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

}