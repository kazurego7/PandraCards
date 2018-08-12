using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	IList<IList<Card>> placedCards;
	[SerializeField] CardPlacer cardPlacer;

	public void Awake () {
		placedCards = new List<IList<Card>> ();
	}

	bool CanPlayCards (IList<Card> playCards) {
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
		var playCardsCanPlayForStrongers = isStrongerColor && playCards.Count == topPlacedCards.Count;

		var isWeakerColor = stronger2ndColors
			.Any (stronger2ndColor =>
				stronger2ndColor.Item1 == topPlacedColor &&
				stronger2ndColor.Item2 == playColor);
		var playCardsCanPlayForWeakers = isWeakerColor && playCards.Count == topPlacedCards.Count + 1;

		var topPlacedCardsAreNoColor = topPlacedColor == Card.Color.NoColor;

		return playCardsCanPlayForStrongers || playCardsCanPlayForWeakers || topPlacedCardsAreNoColor;
	}
	public void PlayCards (IList<Card> cards) {
		foreach (var card in cards) {
			card.transform.SetParent (transform);
		}
		placedCards.Add (cards);
	}

	public void PlayCardsForHands (IList<HandPlace> selectedHandPlaces) {
		var selectedCards = selectedHandPlaces.Select (selected => selected.GetCard ()).ToList ();
		if (CanPlayCards (selectedCards)) {
			PlayCards (selectedCards);
			Debug.Log (selectedHandPlaces.Count);
			cardPlacer.ReplenishHands (selectedHandPlaces);

			foreach (var selectedCard in selectedCards) {
				StartCoroutine (selectedCard.DrawMove (transform.position, moveingFrame : 10));
			}
			StartCoroutine (cardPlacer.DrawReplenishCards (7));
		}
	}

	public IEnumerator DrawFirstCardPlacing () {
		var topPlacedCards = placedCards.First ();
		var placeToPlayArea = topPlacedCards.Select ((card) => StartCoroutine (card.DrawMove (transform.position, moveingFrame : 10)));
		yield return placeToPlayArea.Last ((coroutine) => coroutine != null);
	}
}