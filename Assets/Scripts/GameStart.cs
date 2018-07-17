using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStart : MonoBehaviour {

	[SerializeField] GameObject RedCard;

	[SerializeField] GameObject GreenCard;

	[SerializeField] GameObject BlueCard;

	Int32 cardNum = 10;

	IList<GameObject> cards = new List<GameObject> ();
	IList<T> Shuffle<T> (IList<T> list) {
		// Guidは一意でランダムな値を表す構造体
		return list.OrderBy (i => Guid.NewGuid ()).ToList ();
	}

	IList<GameObject> InitCards () {
		var cards = new List<GameObject> ();
		foreach (var i in new Int32[cardNum]) {
			cards.Add (Instantiate (RedCard));
			cards.Add (Instantiate (GreenCard));
			cards.Add (Instantiate (BlueCard));
		}
		return Shuffle (cards);
	}
	// Use this for initialization
	void Start () {
		this.cards = InitCards ();
		return;
	}

	// Update is called once per frame
	void Update () {

	}
}