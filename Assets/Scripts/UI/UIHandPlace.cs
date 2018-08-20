using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandPlace : MonoBehaviour {

	[SerializeField] Transform handPlace;
	
	UIHandSelector UIHandSelector;
	GameObject frame;
	public bool FrameActivity {
		get;
		private set;
	}

	void Awake () {
		frame = transform.GetChild (0).gameObject;
		SetFrameActive (frame.activeInHierarchy);
		UIHandSelector = transform.GetComponentInParent<UIHandSelector>();
	}

	public HandPlace HandPlace {
		get { return handPlace.GetComponent<HandPlace>(); }
	}

	public void SetFrameActive (bool flag) {
		FrameActivity = flag;
		handPlace.GetComponent<HandPlace>().IsSelcted = flag;
	}
	public void DrawFrame () {
		frame.SetActive (FrameActivity);
	}

	public void SelectFrame() {
		UIHandSelector.SelectFrame(this);
		UIHandSelector.DrawFrames();
	}
}