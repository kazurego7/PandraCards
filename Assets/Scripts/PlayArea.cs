using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	IList<IList<Card>> placedCards;

	public void Awake () {
		placedCards = new List<IList<Card>> ();
	}
	public void PlayCards (IList<Card> cards) {
		foreach (var card in cards) {
			card.transform.SetParent (transform);
		}
		placedCards.Add (cards);
	}

	public IEnumerator DrawFirstCardPlacing () {
		var topPlacedCards = placedCards.First ();
		var placeToPlayArea = topPlacedCards.Select( (card)=> StartCoroutine(card.DrawMove(transform.position)));
		yield return placeToPlayArea.Last((coroutine) => coroutine != null);
	}
}