using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class DiscardBoxPresenter : MonoBehaviour {

	DiscardsBox discardBox;

	public IObservable < (Card, int, Transform) > DicardNotice {
		private set;
		get;
	}
	void Start () {
		DicardNotice = discardBox.DiscardNotice
			.SelectMany (x => x)
			.SelectMany (x => x)
			.Select ((card, i) => (card, i, transform));
	}
}