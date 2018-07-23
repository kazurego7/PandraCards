using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : MonoBehaviour {
	bool IsSelected {
		set;
		get;
	}

	GameObject selectionFrame;
	void Start () {
		selectionFrame = transform.GetChild (0).gameObject;
		Debug.Log (selectionFrame);
	}

	public void ChangeSelect () {
		IsSelected = !IsSelected;
		Debug.Log (this.name + ":" + IsSelected.ToString ());
		DrawSelect ();
	}

	public void DrawSelect () {
		selectionFrame.SetActive (IsSelected);
	}
}