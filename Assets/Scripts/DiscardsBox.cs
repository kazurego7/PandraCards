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
		//DiscardNotice.Value = (discard, this);
	}
	public IEnumerator DrawRemovePlayAreaCards () {
		var startDelaySecond = 0.07f;
		foreach (var discard in Discard.SelectMany (x => x)) {
			StartCoroutine (discard.DrawMove (transform.position, 15).ToYieldInstruction ());
			yield return new WaitForSeconds (startDelaySecond);
		}
		yield return null;
	}
}