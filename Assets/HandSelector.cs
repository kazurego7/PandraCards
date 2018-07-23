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
		var predicate = true; /******************* これから変更したい場所  ****************/
		if (predicate) {
			// 選択をアップデートされたやつだけ、選択
			updated.DrawSelected ();
		} else {
			// 手札全部の選択をキャンセル
			foreach (var selected in handSelecteds) {
				selected.IsSelected = false;
				selected.DrawSelected ();
			}
		}
	}
}