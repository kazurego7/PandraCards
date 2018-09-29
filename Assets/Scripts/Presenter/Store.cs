using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;

public class Store {
	public readonly (OneHandStore[], DeckStore) yourResouce;
	public readonly (OneHandStore[], DeckStore) opponentResouce;
	public readonly (PlayAreaStore, PlayAreaStore) playAreas;
	public readonly DiscardBoxStore discardBox;
	public readonly CardStore[] cards;

	public abstract class Message {
		public class Replenished : Message {
			public readonly (Transform source, Transform target) msg;
		}
		public class Played : Message {
			public readonly (Transform source, Transform target) msg;
		}
	}

	public Store () {

	}

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