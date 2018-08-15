﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardsBox : MonoBehaviour {
	IList<IList<IList<Card>> > discardsList;

	public void Awake () {
		discardsList = new List<IList<IList<Card>> > ();
	}

	public void Add (IList<IList<IList<Card>> > discards) {
		discardsList = discards;
	}

	public IEnumerator DrawRemovePlayAreaCards () {
		var linerDiscards = discardsList.SelectMany (x => x).SelectMany (x => x);
		var startDelaySecond = 0.1f;
		foreach (var discard in linerDiscards) {
			discard.DrawMove(transform.position ,20);
			yield return new WaitForSeconds (startDelaySecond);
		}
		yield return null;
	}
}