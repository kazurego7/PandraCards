using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class OnePlayArea : MonoBehaviour {
	IList<IList<Card>> PlayedCards {
		set;
		get;
	} = new List<IList<Card>> ();

	// public IReactiveProperty < (IList<Card> playCards, OnePlayArea putArea) > PlayNotice {
	// 	private set;
	// 	get;
	// } = new ReactiveProperty < (IList<Card>, OnePlayArea) > ();

	// public IReactiveProperty < (Card firstCard, OnePlayArea putArea) > FirstPutNotice {
	// 	private set;
	// 	get;
	// } = new ReactiveProperty < (Card, OnePlayArea) > ();

	public void Delete () {
		StopAllCoroutines ();
		var linerRemoved = RemoveAll ().SelectMany (x => x);
		foreach (var removed in linerRemoved) {
			Destroy (removed.gameObject);
		}
	}
	public bool CanPlay (IList<Card> playCards) {
		var playColor = playCards.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;

		var topPlacedCards = PlayedCards.LastOrDefault (); // 一番下が最初(first)
		var topPlacedColor = topPlacedCards?.FirstOrDefault ()?.MyColor ?? Card.Color.NoColor;

		var strongColorsThan2nd = new List < (Card.Color, Card.Color) > () {
			(Card.Color.Blue, Card.Color.Red),
			(Card.Color.Red, Card.Color.Green),
			(Card.Color.Green, Card.Color.Blue)
		};

		var isStrongColor = strongColorsThan2nd
			.Any (stronger2ndColor =>
				stronger2ndColor.Item1 == playColor &&
				stronger2ndColor.Item2 == topPlacedColor);
		var playCardsCanPlayForStrongers = isStrongColor && playCards.Count == topPlacedCards.Count;

		var isWeakColor = strongColorsThan2nd
			.Any (stronger2ndColor =>
				stronger2ndColor.Item1 == topPlacedColor &&
				stronger2ndColor.Item2 == playColor);
		var playCardsCanPlayForWeaks = isWeakColor && playCards.Count == topPlacedCards.Count + 1;

		var topPlacedCardsAreNoColor = topPlacedColor == Card.Color.NoColor && playCards.Count == 1;

		return playCardsCanPlayForStrongers || playCardsCanPlayForWeaks || topPlacedCardsAreNoColor; // topが色無しなら無条件で置ける
	}
	public void Play (IList<Card> cards) {
		if (cards == null) return;
		PlayedCards.Add (cards);
		// PlayNotice.Value = (cards, this);
	}

	public void Deal (Card card) {
		if (card == null) return;
		PlayedCards.Add (new List<Card> () { card });
	}

	public IList<IList<Card>> RemoveAll () {
		var discards = new List<IList<Card>> (PlayedCards);
		PlayedCards.Clear ();
		return discards;
	}

	public int CountPlayedCards () {
		return PlayedCards.Aggregate (0, (accm, cards) => accm + cards.Count);
	}

	public bool CanNextPlay (Hand hand) {
		var handCards = hand.GetAllCards ();
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

	public IObservable<Unit> DrawPlay () {
		var prevPlacedCardIndex = (PlayedCards.Count - 1) - 1; // prevPlacedCardIndex = placedCardIndex - 1
		if (prevPlacedCardIndex < 0) return Observable.ReturnUnit ();
		var prevPlacedCardZ = prevPlacedCardIndex < 0 ?
			0 :
			PlayedCards[prevPlacedCardIndex].Last ().transform.position.z; // 注意!! 既にCardはPlayされ、placedCardsに格納されている
		var selectedCards = PlayedCards.Last ();

		return Observable.Range (0, selectedCards.Count)
			.SelectMany (index => {
				var leftmostDistance = 0.2f * (-(selectedCards.Count - 1) + 2 * index);
				var heightVector = (prevPlacedCardZ + Card.thickness * -(index + 1)) * Vector3.forward; // 注意!! 左手座標系(手前の方がマイナス)
				var movePosition = transform.position + leftmostDistance * Vector3.left + heightVector;
				return selectedCards[index].DrawMove (selectedCards[index].transform.position + heightVector)
					.Concat (selectedCards[index].DrawMove (movePosition, moveingFrame : 10));
			});
	}

	public IObservable<Unit> DrawDeal () {
		var topPlacedCards = PlayedCards.FirstOrDefault ();
		if (topPlacedCards == null) return Observable.ReturnUnit ();
		var placeToPlayArea = topPlacedCards.Select ((card) =>
			card.DrawMove (transform.position, moveingFrame : 10)).Merge ();
		return placeToPlayArea;
	}
}