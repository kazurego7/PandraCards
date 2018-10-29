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

	public IObservable<Unit> DrawReplenish () {
		var moveingFrame = 7;
		if (PutCard == null) return Observable.ReturnUnit ();
		var movePosition = transform.position + Card.thickness * Vector3.back;
		return PutCard.DrawMove (movePosition, moveingFrame);
	}

	public IReactiveProperty<bool> IsSelcted {
		get;
	} = new ReactiveProperty<bool> (false);
}