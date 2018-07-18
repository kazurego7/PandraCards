using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour {
	class CardRacipe : Tuple<String, Int32> {
		public String Name { get; }
		public Int32 Number { get; }
		public CardRacipe (String name, Int32 number) : base (name, number) {
			Name = name;
			Number = number;
		}
	}

	readonly String pathForCards = "Prefabs/Cards/";
	IList<GameObject> deck;

	// Use this for initialization
	void Start () {
		DeckInit ();
	}

	// Update is called once per frame
	void Update () {

	}
	void DeckInit () {
		var cardRacipes = new List<CardRacipe> () {
			new CardRacipe ("RedCard", 10),
				new CardRacipe ("BlueCard", 10),
				new CardRacipe ("GreenCard", 10)
		};

		var cards = new List<GameObject> ();
		foreach (var cardRacipe in cardRacipes) {
			var card = Resources.Load<GameObject> (pathForCards + cardRacipe.Name);
			foreach (var item in new int[cardRacipe.Number]) {
				cards.Add (Instantiate<GameObject> (card, transform.position, Quaternion.identity, transform));
			}
		}
		deck = cards;
		Shuffle ();
	}

	bool IsNoCard () {
		return deck.Count == 0;
	}
	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		deck = deck.OrderBy (i => Guid.NewGuid ()).ToList ();
		return;
	}
	public GameObject TopDraw () {
		if (this.IsNoCard ()) {
			return Instantiate (Resources.Load<GameObject> (pathForCards + "EmptyCard"), transform.position, Quaternion.identity, transform);
		} else {
			var top = deck[0];
			deck.RemoveAt (0);
			return top;
		}
	}
}