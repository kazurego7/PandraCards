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
		DrawShuffle ();
	}

	// Update is called once per frame
	void Update () { }
	void DeckInit () {

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
		deck = cards;

		Shuffle ();
		DrawHeightAdjustedCards ();
	}

	bool IsNoCard () {
		return deck.Count == 0;
	}
	void DrawHeightAdjustedCards () {
		// デッキのそれぞれのカードの高さを厚みによって調節
		var cardThickness = 0.0005f; // 遊戯王カードの厚みが0.5mm = 0.0005m
		foreach (var index in Enumerable.Range (0, deck.Count)) {
			deck[index].transform.Translate (0, 0, -index * cardThickness);
		}
	}
	public void Shuffle () {
		// Guidは一意でランダムな値を表す構造体
		deck = deck.OrderBy (i => Guid.NewGuid ()).ToList ();
		return;
	}

	public GameObject TopDraw () {
		if (this.IsNoCard ()) {
			var emptyCard = Resources.Load<GameObject> (pathForCards + "EmptyCard");
			return Instantiate (emptyCard, transform.position, Quaternion.identity, transform);
		} else {
			var top = deck[0];
			deck.RemoveAt (0);
			return top;
		}
	}

	public void DrawShuffle () {
		StartCoroutine (DrawCardsShuffle ());
	}
	IEnumerator DrawCardsShuffle () {
		// 各カードが動き始めるのを何秒遅延するか
		var startDelaySecond = 0.01f;
		foreach (var index in Enumerable.Range (0, deck.Count)) {
			StartCoroutine (DrawKthCardShuffle (deck[index], index));
			yield return new WaitForSeconds (startDelaySecond);
		}
	}

	// ほんとはDrawShuffleの内部関数にしたいが、C#6.0ではできない......
	IEnumerator DrawKthCardShuffle (GameObject card, Int32 kth) {
		// 設定項目
		var moveingFrame = 10;
		var moveVec = card.transform.right * 3f;

		var start = card.transform.position;
		var middle = start + moveVec;
		// middleまでmoveingFrameで動かす
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			card.transform.position = Vector3.Lerp (start, middle, (float) currentFrame / moveingFrame);
			yield return null;
		}
		// startまでmoveingFrameで動かす
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			card.transform.position = Vector3.Lerp (middle, start, (float) currentFrame / moveingFrame);
			yield return null;
		}
	}
}