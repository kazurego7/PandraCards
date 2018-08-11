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
		Debug.Log(CanPlayCards(cards));
		if (CanPlayCards (cards)) {
			foreach (var card in cards) {
				card.transform.SetParent (transform);
			}
			placedCards.Add (cards);
		}
	}

	public bool CanPlayCards (IList<Card> playCards) {
		var playColor = playCards.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;
		
		var topPlacedCards = placedCards?.FirstOrDefault ();
		var topPlacedColor = topPlacedCards?.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;

		var stronger2ndColors = new List<(Card.Color, Card.Color)> () {
			(Card.Color.Blue, Card.Color.Red),
			(Card.Color.Red, Card.Color.Green),
			(Card.Color.Green, Card.Color.Blue)
		};

		var isStrongerColor = stronger2ndColors
			.Any (stronger2ndColor =>
				stronger2ndColor.Item1 == playColor &&
				stronger2ndColor.Item2 == topPlacedColor);
		var canPlayStrongers = isStrongerColor && playCards.Count == topPlacedCards.Count;

		var isWeakerColor = stronger2ndColors
			.Any (stronger2ndColor =>
				stronger2ndColor.Item1 == topPlacedColor &&
				stronger2ndColor.Item2 == playColor);
		var canPlayWeakers = isWeakerColor && playCards.Count == topPlacedCards.Count + 1;

		var topPlacedCardIsNoColor = topPlacedColor == Card.Color.NoColor;

		return canPlayStrongers || canPlayWeakers || topPlacedCardIsNoColor;
	}

	public IEnumerator DrawFirstCardPlacing () {
		var topPlacedCards = placedCards.First ();
		var placeToPlayArea = topPlacedCards.Select ((card) => StartCoroutine (card.DrawMove (transform.position)));
		yield return placeToPlayArea.Last ((coroutine) => coroutine != null);
	}
}