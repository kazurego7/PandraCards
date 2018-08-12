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

		var topPlacedCards = placedCards?.LastOrDefault (); // 一番下が最初(first)
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

		return playCardsCanPlayForStrongers || playCardsCanPlayForWeakers || topPlacedCardsAreNoColor; // topが色無しなら無条件で置ける
	}
	public void PlayCards (IList<Card> cards) {
		foreach (var card in cards) {
			card.transform.SetParent (transform);
		}
		placedCards.Add (cards);
	}

	public void PlayCardsForHands (IList<HandPlace> selectedHandPlaces) {
		var selectedCards = selectedHandPlaces.Select (selected => selected.GetCard ()).ToList ();
		IEnumerator DrawCardPlayMoves () {
			var prevPlacedCardZ = placedCards[placedCards.Count-2].Last()?.transform.position.z ?? 0; // 注意!! 既にCardはPlayされ、placedCardsに格納されている
			foreach (var index in Enumerable.Range(0, selectedCards.Count)) {
				var leftmostDistance = 0.2f * (-(selectedCards.Count - 1) + 2 * index);
				var heightVector = (prevPlacedCardZ + -Card.thickness * (index + 1)) * Vector3.forward; // 注意!! 左手座標系(手前の方がマイナス)
				var movePosition = transform.position + leftmostDistance * Vector3.left + heightVector;
				StartCoroutine (selectedCards[index].DrawMove (movePosition, moveingFrame : 10));
			}
			yield return null;
		}
		if (CanPlayCards (selectedCards)) {
			PlayCards (selectedCards);
			cardPlacer.ReplenishHands (selectedHandPlaces);

			StartCoroutine (DrawCardPlayMoves ());
			StartCoroutine (cardPlacer.DrawReplenishCards (7));
		}
	}

	public IEnumerator DrawFirstCardPlacing () {
		var topPlacedCards = placedCards.First ();
		var placeToPlayArea = topPlacedCards.Select ((card) => StartCoroutine (card.DrawMove (transform.position, moveingFrame : 10)));
		yield return placeToPlayArea.Last ((coroutine) => coroutine != null);
	}
}