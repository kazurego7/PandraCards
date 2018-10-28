using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	IList<OnePlayArea> onePlayAreas;
	bool CanNextPlay (Hand hand) {
		return onePlayAreas.Any (onePlayArea =>
			onePlayArea.CanNextPlay (hand));
	}

	IList<IList<Card>> RemovePlayAreaCards () {
		return onePlayAreas.SelectMany (onePlayArea => onePlayArea.RemoveAll ()).ToList ();
	}
}