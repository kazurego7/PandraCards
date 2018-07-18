using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardRacipe = System.Tuple<System.String, System.Int32>;

public class GameStart : MonoBehaviour {
	class CardRacipe : Tuple<String, Int32> {
		public String Name { get; }
		public Int32 Number { get; }
		public CardRacipe (String name, Int32 number) : base (name, number) {
			Name = name;
			Number = number;
		}
	}
	class FieldDeck {
		IList<GameObject> holder = new List<GameObject> ();
		GameObject emptyCard;
		public FieldDeck (IList<CardRacipe> cardRacipes) {
			var cards = new List<GameObject> ();
			foreach (var cardRacipe in cardRacipes) {
				var cardObj = Resources.Load<GameObject> ("Prefabs/Cards/" + cardRacipe.Name);
				foreach (var item in new int[cardRacipe.Number]) {
					cards.Add (Instantiate (cardObj));
				}
			}
			holder = cards;
			Shuffle ();
			emptyCard = Resources.Load<GameObject> ("Prefabs/Cards/EmptyCard");
		}
		bool IsEmpty<T> (IList<T> list) {
			return list.Count == 0;
		}
		public void Shuffle () {
			// Guidは一意でランダムな値を表す構造体
			holder = holder.OrderBy (i => Guid.NewGuid ()).ToList ();
			return;
		}
		public GameObject TopDraw () {
			if (IsEmpty (holder)) {
				return Instantiate (emptyCard);
			} else {
				var top = holder[0];
				holder.RemoveAt (0);
				return top;
			}
		}
	}

	FieldDeck yourDeck;
	FieldDeck opponentDeck;

	FieldDeck DeckInit () {

		var cardAndNums = new List<CardRacipe> () {
			new CardRacipe ("redCard", 10),
				new CardRacipe ("blueCard", 10),
				new CardRacipe ("greenCard", 10)
		};
		return new FieldDeck (cardAndNums);
	}

	// Use this for initialization
	void Start () {
		yourDeck = DeckInit ();
		opponentDeck = DeckInit ();
	}

	// Update is called once per frame
	void Update () {

	}
}