using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class DeckPresenter : MonoBehaviour {

	Deck deck;

	public IObservable < (Card, int, Transform) > ShuffleNotice {
		private set;
		get;
	}

	void Start () {
		ShuffleNotice = deck.ShuffledNotice
			.SelectMany (cards => cards.Select ((card, i) => (card, i, transform)));
	}
}