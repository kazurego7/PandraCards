using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class OneHand : MonoBehaviour {

	public Card PutCard {
		get;
		private set;
	}

	public IReactiveProperty < (Card mover, OneHand target) > ReplenishedNotice {
		get;
	} = new ReactiveProperty < (Card, OneHand) > ();

	public bool Replenish (Card card) {
		if (card == null) return false;
		PutCard = card;
		ReplenishedNotice.Value = (card, this);
		return true;
	}

	public Card RemoveCard () {
		var removed = PutCard;
		PutCard = null;
		return removed;
	}

	public IEnumerator DrawPutCards (int moveingFrame) {
		if (PutCard == null) yield break;
		var movePosition = transform.position + Card.thickness * Vector3.back;
		yield return StartCoroutine (PutCard.DrawMove (movePosition, moveingFrame));
	}

	public ReactiveProperty<bool> IsSelcted {
		get;
	} = new ReactiveProperty<bool> (false);
}