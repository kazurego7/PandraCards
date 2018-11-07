using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IOOneHand : MonoBehaviour {
	OneHand thisHand;
	IList<OneHand> otherHands;
	Hand hand;
	Drawable drawable;

	void Awake () {
		thisHand = GetComponent<OneHand> ();
		hand = GetComponentInParent<Hand> ();
		otherHands = hand.GetComponentsInChildren<OneHand> ()
			.Where (otherHand => otherHand != thisHand)
			.ToList ();
		drawable = GetComponentInParent<Drawable> ();
	}

	void Start () {
		this.OnMouseDownAsObservable ()
			.Subscribe (_ => {
				// IsSelected == true ならば PutCard != null
				// ゆえに、IsSelected == true ならば Card.MyColor != null

				// 選んだ場所にカードがなければ、選択できない
				if (thisHand.PutCard == null) {
					return;
				}

				// 選んだカードが選択されていれば、非選択に
				if (thisHand.IsSelected) {
					thisHand.DeselectFrame ();
					drawable.SyncCommand.Execute (thisHand.DrawFrame ());
					return;
				}

				// 1枚目のカードを選ぶ
				var selecteds = otherHands.Where (other => other.IsSelected);
				if (!selecteds.Any ()) {
					thisHand.SelectFrame ();
					drawable.SyncCommand.Execute (thisHand.DrawFrame ());
					return;
				}

				// 2枚目以降のカードを選ぶ
				var selectedColor = selecteds
					.Select (selected => selected.PutCard.MyColor)
					.First ();
				var selectColor = thisHand.PutCard.MyColor;
				if (selectedColor == selectColor) {
					thisHand.SelectFrame ();
					drawable.SyncCommand.Execute (thisHand.DrawFrame ());
				} else {
					foreach (var selected in selecteds) {
						selected.DeselectFrame ();
					}
					thisHand.SelectFrame ();
					var drawDeselectFrame = otherHands
						.Select (other => other.DrawFrame ())
						.Merge ();
					var drawSelectFrame = thisHand.DrawFrame ();
					var drawFrame = drawDeselectFrame.Merge (drawSelectFrame);
					drawable.SyncCommand.Execute (drawFrame);
				}
			});
	}

}