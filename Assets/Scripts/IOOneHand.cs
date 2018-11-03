using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IOOneHand : MonoBehaviour {
	OneHand oneHand;
	Hand hand;
	Drawable drawable;

	void Awake () {
		oneHand = GetComponent<OneHand> ();
		hand = GetComponentInParent<Hand> ();
		drawable = GetComponentInParent<Drawable> ();
	}

	void Start () {
		this.OnMouseDownAsObservable ()
			.Subscribe (_ => {
				hand.SelectFrame (oneHand);
				drawable.SyncCommand.Execute (oneHand.DrawFrame ());
			});
	}

}