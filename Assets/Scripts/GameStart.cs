using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStart : MonoBehaviour {
	[SerializeField] GameObject emptyCard;

	public class FieldDeck {
		IList<GameObject> holder = new List<GameObject> ();
		GameObject emptyCard;
		public FieldDeck (IList<Tuple<GameObject, Int32>> cardObjAndNums) {
			var cards = new List<GameObject> ();
			foreach (var cardAndNum in cardObjAndNums) {
				var card = cardAndNum.Item1;
				var num = cardAndNum.Item2;
				foreach (var item in new int[num]) {
					cards.Add (Instantiate (card));
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

	FieldDeck myDeck;

	// Use this for initialization
	void Start () {
		var redCard = Resources.Load<GameObject> ("Prefabs/Cards/RedCard");
		var blueCard = Resources.Load<GameObject> ("Prefabs/Cards/BlueCard");
		var greenCard = Resources.Load<GameObject> ("Prefabs/Cards/GreenCard");

		var cardAndNums = new List<Tuple<GameObject, Int32>> () {
				new Tuple<GameObject, Int32> (redCard, 10),
					new Tuple<GameObject, Int32> (blueCard, 10),
					new Tuple<GameObject, Int32> (greenCard, 10)
			};
		var myDeck = new FieldDeck (cardAndNums);
		Debug.Log (myDeck.TopDraw ());
	}

	// Update is called once per frame
	void Update () {

	}
}