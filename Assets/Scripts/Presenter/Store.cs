using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniRx;
using UnityEngine;

public class Store {
	public readonly OneHandStore[] yourHand;
	public readonly DeckStore yourDeck;
	public readonly OneHandStore[] opponentHand;
	public readonly DeckStore opponentDeck;
	public readonly (PlayAreaStore, PlayAreaStore) playAreas;
	public readonly DiscardBoxStore discardBox;
	public readonly CardStore[] cards;
	public readonly IObservable<Message> messager;

	public Store () {
		public readonly struct OneHandStore {
			public readonly Transform transform;
			public readonly OneHand onehand;
		}
		public readonly struct DeckStore {
			public readonly Transform transform;
			public readonly Deck deck;
		}
		public readonly struct PlayAreaStore {
			public readonly PlayArea playArea;
			public readonly Transform transform;
		}

		public readonly struct DiscardBoxStore {
			public readonly DiscardsBox discardBox;
			public readonly Transform transform;
		}
		public readonly struct CardStore {
			public readonly Card card;
			public readonly Transform transform;
		}
	}