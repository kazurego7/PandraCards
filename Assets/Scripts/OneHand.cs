using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class OneHand : MonoBehaviour {

	GameObject selectFrame;

	public Card PutCard {
		get;
		private set;
	}

	public bool IsSelected {
		get;
		set;
	}

	public IReactiveProperty < (Card movee, OneHand target) > ReplenishedNotice {
		get;
	} = new ReactiveProperty < (Card, OneHand) > ();

	void Awake () {
		selectFrame = transform.GetChild (0).gameObject;
	}
	public bool Deal (Card card) {
		if (card == null) return false;
		PutCard = card;
		ReplenishedNotice.Value = (card, this);
		return true;
	}

	public Card Remove () {
		var removed = PutCard;
		PutCard = null;
		return removed;
	}

	public void SelectFrame () {
		IsSelected = true;
	}

	public void DeselectFrame () {
		IsSelected = false;
	}

	public IObservable<Unit> DrawDeal () {
		var moveingFrame = 7;
		if (PutCard == null) return Observable.ReturnUnit ();
		var movePosition = transform.position + Card.thickness * Vector3.back;
		return PutCard.DrawMove (movePosition, moveingFrame);
	}

	public IObservable<Unit> DrawFrame () {
		return Observable.ReturnUnit ().Do (_ => {
			selectFrame.SetActive (IsSelected);
		});
	}
}