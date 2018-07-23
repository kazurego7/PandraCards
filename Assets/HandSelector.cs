using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandSelector : MonoBehaviour {
	IList<HandSelected> handSelecteds;
	void Start () {
		handSelecteds = GetComponentsInChildren<HandSelected> ().OrderBy ((t => t.name)).ToList ();
	}
	public void UpdateSelected (HandSelected updated) {
		var predicate = true;
		if (predicate) {
			updated.DrawSelected ();
		} else {
			foreach (var selected in handSelecteds) {
				selected.IsSelected = false;
				selected.DrawSelected ();
			}
		}
	}
}