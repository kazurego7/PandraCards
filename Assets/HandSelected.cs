using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSelected : MonoBehaviour {
	public bool IsSelected {
		set;
		get;
	}
	HandSelector handSelector;
	GameObject selectionFrame;
	[SerializeField] Transform handPlace;
	void Start () {
		handSelector = transform.GetComponentInParent<HandSelector> ();
		selectionFrame = transform.GetChild (0).gameObject;
		IsSelected = selectionFrame.activeInHierarchy;
		//Debug.Log (selectionFrame);
	}

	public void Select () {
		if (IsSelected) {
			IsSelected = false;
			DrawSelected ();
		} else {
			IsSelected = true;
			handSelector.UpdateSelected (this);
		}
	}

	public void DrawSelected () {
		selectionFrame.SetActive (IsSelected);
	}
}