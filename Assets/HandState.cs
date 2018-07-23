using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : MonoBehaviour {
	public bool IsSelected {
		set;
		get;
	}

	public void ChangeSelect () {
		IsSelected = !IsSelected;
		Debug.Log (this.name + ":" + IsSelected.ToString ());
	}

	public void DrawSelect () {
		if (IsSelected) {
			// UI表示
		} else {
			// UI非表示
		}
	}
}