using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class DiscardsBox : MonoBehaviour {
	public IList<IList<Card>> Discard {
		get;
		private set;
	} = new List<IList<Card>> ();

	public ReactiveProperty < (IList<IList<Card>> discards, DiscardsBox dustBin) > DiscardNotice {
		get;
		private set;
	}

	public void Delete () {
		foreach (var discard in Discard.SelectMany (x => x)) {
			Destroy (discard.gameObject);
		}
	}

	public void Store (IList<IList<Card>> discard) {
		Discard = Discard.Concat (discard).ToList ();
		foreach (var dc in Discard) {
			Debug.Log (dc[0]);
		}
		//DiscardNotice.Value = (discard, this);
	}

	public IObservable<Unit> DrawRemoveForPlayArea () {
		var startDelayFrame = 1;
		var discards = Discard.SelectMany (x => x).ToList ();
		return Observable
			.IntervalFrame (startDelayFrame).Take (discards.Count)
			.Select (count => {
				return discards[(int) count].DrawMove (transform.position, 15);
			})
			.Merge ();
	}
}