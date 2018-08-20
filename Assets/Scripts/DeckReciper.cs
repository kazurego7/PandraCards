﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckReciper : MonoBehaviour {
	class CardRecipe : Tuple<String, Int32> {
		public String Name { get; }
		public Int32 Number { get; }
		public CardRecipe (String name, Int32 number) : base (name, number) {
			Name = name;
			Number = number;
		}
	}

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
		var tmpCardRecipes = new List<CardRecipe> () {
			new CardRecipe ("RedCard", RedNum),
				new CardRecipe ("BlueCard", BlueNum),
				new CardRecipe ("GreenCard", GreenNum)
		};

		// デッキレシピから、デッキのリストを作成
		var cards = new List<Card> ();
		foreach (var cardRecipe in tmpCardRecipes) {
			var card = Resources.Load<GameObject> (pathForCards + cardRecipe.Name);
			foreach (var item in Enumerable.Range (0, cardRecipe.Number)) {
				cards.Add (Instantiate<GameObject> (card, transform).GetComponent<Card> ());
			}
		}
		return cards;
	}
}