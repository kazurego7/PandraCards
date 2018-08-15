using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	IList<IList<Card>> placedCards;
	FieldPlacer fieldPlacer;

	public void Awake () {
		placedCards = new List<IList<Card>> ();
		fieldPlacer = GetComponentInParent<FieldPlacer> ();
	}

	bool CanPlayCards (IList<Card> playCards) {
		var playColor = playCards.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;

		var topPlacedCards = placedCards.LastOrDefault (); // 一番下が最初(first)
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

		var topPlacedCardsAreNoColor = topPlacedColor == Card.Color.NoColor && playCards.Count == 1;

		return playCardsCanPlayForStrongers || playCardsCanPlayForWeakers || topPlacedCardsAreNoColor; // topが色無しなら無条件で置ける
	}
	public void PlayCards (IList<Card> cards) {
		foreach (var card in cards) {
			card.transform.SetParent (transform);
		}
		if (cards != null) placedCards.Add (cards);
	}

	public IList<IList<Card>> RemovePlacedCards () {
		var discards = placedCards;
		placedCards = new List<IList<Card>> ();
		return discards;
	}

	public void DebugLogPlacedCards () {
		var cardColorAndNums = placedCards.Select (cards => (cards.First ().MyColor, cards.Count));
		foreach (var cardColorAndNum in cardColorAndNums) {
			Debug.Log ($"{cardColorAndNum.Item1} : {cardColorAndNum.Item2}");
		}
	}
	public void PlayCardsForHands (IList<HandPlace> selectedHandPlaces, CardPlacer cardPlacer) {
		var selectedCards = selectedHandPlaces.Select (selected => selected.PlacedCard).Where(selected => selected != null).ToList ();
		IEnumerator DrawCardPlayMoves () {
			var prevCardIndex = placedCards.Count - 2;
			var prevPlacedCardZ = prevCardIndex >= 0 ?
				placedCards[prevCardIndex].Last ()?.transform.position.z ?? 0 : // 注意!! 既にCardはPlayされ、placedCardsに格納されている
				0;
			foreach (var index in Enumerable.Range (0, selectedCards.Count)) {
				var leftmostDistance = 0.2f * (-(selectedCards.Count - 1) + 2 * index);
				var heightVector = (prevPlacedCardZ + -Card.thickness * (index + 1)) * Vector3.forward; // 注意!! 左手座標系(手前の方がマイナス)
				var movePosition = transform.position + leftmostDistance * Vector3.left + heightVector;
				selectedCards[index].DrawMove (selectedCards[index].transform.position + heightVector, moveingFrame : 1);
				selectedCards[index].DrawMove (movePosition, moveingFrame : 10);
			}
			yield return null;
		}
		if (CanPlayCards (selectedCards)) {
			PlayCards (selectedCards);
			cardPlacer.ReplenishHands (selectedHandPlaces);

			StartCoroutine (DrawCardPlayMoves ());
			StartCoroutine (cardPlacer.DrawReplenishCards (7));

			fieldPlacer.JudgeCanNextPlay ();
		}
	}

	public bool ExistPlayableCards (CardPlacer cardPlacer) {
		var handCards = cardPlacer.GetHands ();
		var playAreaCards = placedCards.LastOrDefault ();
		var playAreaCardsColor = playAreaCards?.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;
		var stronger2ndColors = new List<(Card.Color, Card.Color)> () {
			(Card.Color.Blue, Card.Color.Red),
			(Card.Color.Red, Card.Color.Green),
			(Card.Color.Green, Card.Color.Blue),
		};

		var existHands = handCards.Count > 0;

		var strongerColor = stronger2ndColors.FirstOrDefault (stronger2ndColor => stronger2ndColor.Item2 == playAreaCardsColor).Item1;
		var canPlayStronger = (handCards?.Where (hand => hand.MyColor == strongerColor)?.Count () ?? 0) >= (playAreaCards?.Count () ?? 0);

		var weakerColor = stronger2ndColors.FirstOrDefault (stronger2ndColor => stronger2ndColor.Item1 == playAreaCardsColor).Item2;
		var canPlayWeaker = (handCards?.Where (hand => hand.MyColor == weakerColor)?.Count () ?? 0) >= (playAreaCards?.Count () ?? 0) + 1;

		var playAreaIsNoColor = playAreaCardsColor == Card.Color.NoColor;

		//Debug.Log($"{gameObject.name} : {canPlayStronger}, {canPlayWeaker}, {playAreaIsNoColor}");

		return existHands && (canPlayStronger || canPlayWeaker || playAreaIsNoColor);
	}

	public IEnumerator DrawFirstCardPlacing () {
		var topPlacedCards = placedCards.FirstOrDefault ();
		var placeToPlayArea = topPlacedCards?.Select ((card) => {
			card?.DrawMove (transform.position, moveingFrame : 10);
			return card?.Moveing;
		});
		yield return placeToPlayArea?.LastOrDefault ((coroutine) => coroutine != null);
	}
}