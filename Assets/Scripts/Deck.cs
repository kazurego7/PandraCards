using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour{
	class CardRacipe : Tuple<String, Int32> {
		public String Name { get; }
		public Int32 Number { get; }
		public CardRacipe (String name, Int32 number) : base (name, number) {
			Name = name;
			Number = number;
		}
	}

	readonly String pathForCards = "Prefabs/Cards/";
	IList<GameObject> cards;

	public void Initialize () {

		// 仮のデッキレシピを作成 *** いずれ消す
		var tmpCardRacipes = new List<CardRacipe> () {
			new CardRacipe ("RedCard", 10),
				new CardRacipe ("BlueCard", 10),
				new CardRacipe ("GreenCard", 10)
		};

		// デッキレシピから、デッキのリストを作成
		var cards = new List<GameObject> ();
		foreach (var cardRacipe in tmpCardRacipes) {
			var card = Resources.Load<GameObject> (pathForCards + cardRacipe.Name);
			foreach (var item in Enumerable.Range (0, cardRacipe.Number)) {
				cards.Add (Instantiate<GameObject> (card, transform));
			}
		}
		this.cards = cards;

		Shuffle ();
		StartCoroutine(DrawHeightAdjustedCards ());
	}

	public void Delete () {
		StopAllCoroutines ();
		foreach (var card in cards) {
			Destroy (card);
		}
	}

	bool IsNoCard () {
		return cards.Count == 0;
	}
	public IEnumerator DrawHeightAdjustedCards () {
		// デッキのそれぞれのカードの高さを厚みによって調節
		foreach (var index in Enumerable.Range (0, cards.Count)) {
			var heightAjustedPosition = cards[index].transform.position + index * Card.thickness * Vector3.back;
			StartCoroutine(cards[index].GetComponent<Card>().DrawMove(heightAjustedPosition));
		}
		yield return null;
	}

	public Card TopDraw () {
		if (this.IsNoCard ()) {
			var emptyCard = Resources.Load<GameObject> (pathForCards + "EmptyCard");
			return Instantiate (emptyCard, transform.position, Quaternion.identity, transform).GetComponent<Card>();
		} else {
			var top = cards.Last().GetComponent<Card>();
			cards.RemoveAt (cards.Count - 1);
			return top;
		}
	}

	/*********************************************
	/*	シャッフル系
	/*********************************************/
	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		cards = cards.OrderBy (i => Guid.NewGuid ()).ToList ();
		return;
	}

	public IEnumerator DrawShuffle () {
		// 各カードが動き始めるのを何秒遅延するか
		var startDelaySecond = 0.01f;
		foreach (var index in Enumerable.Range (0, cards.Count - 1)) {
			StartCoroutine (cards[index].GetComponent<Card>().DrawShuffle());
			yield return new WaitForSeconds (startDelaySecond);
		}
		// 最後のコルーチンだけ返すことで、順序処理が行える
		var lastIndex = cards.Count - 1;
		yield return StartCoroutine (cards[lastIndex].GetComponent<Card>().DrawShuffle());
	}
}