using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IOOneHand : MonoBehaviour {
	OneHand oneHand;
	IList<OneHand> otherHands;
	Hand hand;
	Drawable drawable;

	void Awake () {
		oneHand = GetComponent<OneHand> ();
		hand = GetComponentInParent<Hand> ();
		otherHands = hand.GetComponentsInChildren<OneHand> ()
			.Where (otherHand => otherHand != oneHand)
			.ToList ();
		drawable = GetComponentInParent<Drawable> ();
	}

	void Start () {
		this.OnMouseDownAsObservable ()
			.Subscribe (_ => {
				var selectColor = oneHand.PutCard?.MyColor;
				if (selectColor == null) {
					return;
				}

				var selecteds = otherHands.Where (other => other.IsSelected);
				if (!selecteds.Any ()) {
					oneHand.SelectFrame ();
					drawable.SyncCommand.Execute (oneHand.DrawFrame ());
					return;
				}

				var selectedColor = selecteds
					.Select (selected => selected.PutCard.MyColor)
					.First ();

				if (selectedColor == selectColor) {
					oneHand.SelectFrame ();
					drawable.SyncCommand.Execute (oneHand.DrawFrame ());
				} else {
					foreach (var selected in selecteds) {
						selected.DeselectFrame ();
					}
					oneHand.SelectFrame ();

					var drawDeselectFrames = selecteds
						.Select (selected => selected.DrawFrame ())
						.Merge ();
					var drawSelectFrame = oneHand.DrawFrame ();
					drawable.SyncCommand.Execute (drawDeselectFrames.Merge (drawSelectFrame));
				}
			});
	}

}