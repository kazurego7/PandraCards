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

	public bool CanPlayCards (IList<Card> playCard) {
		var playColor = playCard.First ().MyColor;

		var topPlacedCards = placedCards.First ();
		var topPlacedColor = topPlacedCards.First ().MyColor;

		var stronger2ndColors = new List<(Card.Color, Card.Color)> () {
			(Card.Color.Blue, Card.Color.Red),
			(Card.Color.Red, Card.Color.Green),
			(Card.Color.Green, Card.Color.Blue)
		};
		
		var isStrongerColor = stronger2ndColors.Any (stronger2ndColor => stronger2ndColor.Item1 == playColor && stronger2ndColor.Item2 == topPlacedColor);
		var canPlayStrongers = isStrongerColor && playCard.Count == topPlacedCards.Count;

		var isWeakerColor = stronger2ndColors.Any (stronger2ndColor => stronger2ndColor.Item2 == playColor && stronger2ndColor.Item1 == topPlacedColor);
		var canPlayWeakers = isWeakerColor && playCard.Count == topPlacedCards.Count + 1;

		return canPlayStrongers && canPlayWeakers;
	}

	public IEnumerator DrawFirstCardPlacing () {
		var topPlacedCards = placedCards.First ();
		var placeToPlayArea = topPlacedCards.Select ((card) => StartCoroutine (card.DrawMove (transform.position)));
		yield return placeToPlayArea.Last ((coroutine) => coroutine != null);
	}
}