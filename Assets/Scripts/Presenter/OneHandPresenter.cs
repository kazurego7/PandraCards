using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class OneHandPresenter : MonoBehaviour {

	OneHand oneHand;

	public IObservable < (Card, Transform) > ReplenishedNotice {
		private set;
		get;
	}

	void Start () {
		ReplenishedNotice = oneHand.ReplenishedNotice
			.Select (card => (card, target : transform));
	}
}