using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckReciper : MonoBehaviour {

	[SerializeField] int _redNum = 10;
	public int RedNum {
		get;
		set;
	}

	[SerializeField] int _blueNum = 10;
	public int BlueNum {
		get;
		set;
	}

	[SerializeField] int _greenNum = 10;
	public int GreenNum {
		get;
		set;
	}

	readonly String pathForCards = "Prefabs/Cards/";

	void Awake () {
		RedNum = _redNum;
		BlueNum = _blueNum;
		GreenNum = _greenNum;
	}

	public IList<Card> CreateDeck () {
		// 仮のデッキレシピを作成 *** いずれ消す
		var tmpCardRecipes = new List<(String name, Int32 number)> () {
			("RedCard", RedNum),
			("BlueCard", BlueNum),
			("GreenCard", GreenNum)
		};

		// デッキレシピから、デッキのリストを作成
		var cards = new List<Card> ();
		foreach (var cardRecipe in tmpCardRecipes) {
			var card = Resources.Load<GameObject> (pathForCards + cardRecipe.name);
			foreach (var item in Enumerable.Range (0, cardRecipe.number)) {
				cards.Add (Instantiate<GameObject> (card, transform).GetComponent<Card> ());
			}
		}
		return cards;
	}
}