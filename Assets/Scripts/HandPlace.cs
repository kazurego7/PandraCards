using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class HandPlace : MonoBehaviour {


	public Card PutCard {
		get {
			return CardPutNotice.Value;
		}
		set {
			CardPutNotice.Value = value;
		}
	}

	public ReactiveProperty<Card> CardPutNotice {
		get;
		private set;
	} = new ReactiveProperty<Card>();

	public Card RemoveCard () {
		var removed = PutCard;
		PutCard = null;
		return removed;
	}

	public IEnumerator DrawReplaceCards (int moveingFrame) {
		if (PutCard == null) yield break;
		var movePosition = transform.position + Card.thickness * Vector3.back;
		yield return StartCoroutine (PutCard.DrawMove (movePosition, moveingFrame));
	}

	public bool IsSelcted {
		get;
		set;
	}
}