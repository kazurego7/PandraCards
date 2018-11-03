using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandPlace : MonoBehaviour {

	[SerializeField] OneHand oneHand;

	UIHandSelector UIHandSelector;
	GameObject frame;
	public bool FrameActivity {
		get;
		private set;
	}

	void Awake () {
		frame = transform.GetChild (0).gameObject;
		SetFrameActive (frame.activeInHierarchy);
		UIHandSelector = transform.GetComponentInParent<UIHandSelector> ();
	}

	public OneHand OneHnad {
		get { return oneHand; }
	}

	public void SetFrameActive (bool flag) {
		oneHand.IsSelected = flag;
		FrameActivity = flag;
	}
	public void DrawFrame () {
		frame.SetActive (FrameActivity);
	}

	public void SelectFrame () {
		UIHandSelector.SelectFrame (this);
		UIHandSelector.DrawFrames ();
	}
}