using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
public class PlayAreaPresenter : MonoBehaviour {
	PlayArea playArea;
	public IObservable < (Card, int, Transform) > PlayedNotice {
		private set;
		get;
	}

	public IObservable < (Card, Transform) > FirstPutNotice {
		private set;
		get;
	}

	void Start () {
		PlayedNotice = playArea.PlayedNotice
			.SelectMany (cards => cards.Select ((card, i) => (card, i, transform)));
		FirstPutNotice = playArea.FirstPutNotice
			.Select (card => (card, transform));
	}
}