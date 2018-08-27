using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	public ReactiveCollection<IList<Card>> PlayedCards {
		get;
	} = new ReactiveCollection<IList<Card>> ();
	FieldPlacer fieldPlacer;

	public void Awake () {
		fieldPlacer = GetComponentInParent<FieldPlacer> ();
	}

	public void Delete () {
		StopAllCoroutines ();
		var linerRemoved = RemovePlacedCards ().SelectMany (x => x);
		foreach (var removed in linerRemoved) {
			Destroy (removed.gameObject);
		}
	}

	public bool CanPlayCards (IList<Card> playCards) {
		var playColor = playCards.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;

		var topPlacedCards = PlayedCards.LastOrDefault (); // 一番下が最初(first)
		var topPlacedColor = topPlacedCards?.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;

		var stronger2ndColors = new List < (Card.Color, Card.Color) > () {
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
	public bool PlayCards (IList<Card> cards) {
		if (cards == null) return false;
		PlayedCards.Add (cards);
		return true;
	}

	public IList<IList<Card>> RemovePlacedCards () {
		var discards = new List<IList<Card>> (PlayedCards);
		PlayedCards.Clear ();
		return discards;
	}

	public bool ExistPlayableCards (Hand hand) {
		var handCards = hand.GetCardsAll ();
		var playAreaCards = PlayedCards.LastOrDefault ();
		var playAreaCardsColor = playAreaCards?.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;
		var stronger2ndColors = new List < (Card.Color, Card.Color) > () {
			(Card.Color.Blue, Card.Color.Red),
			(Card.Color.Red, Card.Color.Green),
			(Card.Color.Green, Card.Color.Blue),
		};

		var existHands = handCards.Count > 0;

		var strongerColor = stronger2ndColors.FirstOrDefault (stronger2ndColor => stronger2ndColor.Item2 == playAreaCardsColor).Item1;
		var canPlayStronger = (handCards?.Where (handCard => handCard.MyColor == strongerColor)?.Count () ?? 0) >= (playAreaCards?.Count () ?? 0);

		var weakerColor = stronger2ndColors.FirstOrDefault (stronger2ndColor => stronger2ndColor.Item1 == playAreaCardsColor).Item2;
		var canPlayWeaker = (handCards?.Where (handCard => handCard.MyColor == weakerColor)?.Count () ?? 0) >= (playAreaCards?.Count () ?? 0) + 1;

		var playAreaIsNoColor = playAreaCardsColor == Card.Color.NoColor;

		//Debug.Log($"{gameObject.name} : {canPlayStronger}, {canPlayWeaker}, {playAreaIsNoColor}");

		return existHands && (canPlayStronger || canPlayWeaker || playAreaIsNoColor);
	}

	public IEnumerator DrawCardPlayMoves () {
		var prevPlacedCardIndex = (PlayedCards.Count - 1) - 1; // prevPlacedCardIndex = placedCardIndex - 1
		var prevPlacedCardZ = prevPlacedCardIndex >= 0 ?
			PlayedCards[prevPlacedCardIndex].Last ().transform.position.z : // 注意!! 既にCardはPlayされ、placedCardsに格納されている
			0;
		var selectedCards = PlayedCards.Last ();
		foreach (var index in Enumerable.Range (0, selectedCards.Count)) {
			var leftmostDistance = 0.2f * (-(selectedCards.Count - 1) + 2 * index);
			var heightVector = (prevPlacedCardZ + -Card.thickness * (index + 1)) * Vector3.forward; // 注意!! 左手座標系(手前の方がマイナス)
			var movePosition = transform.position + leftmostDistance * Vector3.left + heightVector;
			StartCoroutine (selectedCards[index].DrawMove (selectedCards[index].transform.position + heightVector));
			StartCoroutine (selectedCards[index].DrawMove (movePosition, moveingFrame : 10));
		}
		yield return null;
	}

	public IEnumerator DrawCardPlacing () {
		var topPlacedCards = PlayedCards.FirstOrDefault ();
		var placeToPlayArea = topPlacedCards?.Select ((card) => {
			return StartCoroutine (card?.DrawMove (transform.position, moveingFrame : 10));
		});
		yield return placeToPlayArea?.LastOrDefault ((coroutine) => coroutine != null);
	}
}