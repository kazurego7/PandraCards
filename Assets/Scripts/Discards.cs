using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Discards : MonoBehaviour {

	IList<IList<IList<Card>> > discardsBox;

	void Awake () {
		discardsBox = new List<IList<IList<Card>> > ();
	}

	void Add (IList<IList<IList<Card>> > discards) {
		discardsBox = discards;
	}

	IEnumerator DrawRemovePlayAreaCards () {
		var linerDiscards = discardsBox.SelectMany (x => x).SelectMany (x => x);
		Debug.Log (linerDiscards.Count ());
		foreach (var discard in linerDiscards) {
			yield return discard.Moveing;
			Destroy (discard.gameObject);
		}
		yield return null;
	}
}