using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandPlace : MonoBehaviour {

	[SerializeField] Transform handPlace;
	GameObject frame;
	public bool FrameActivity {
		get;
		private set;
	}

	void Awake () {
		frame = transform.GetChild (0).gameObject;
		SetFrameActive (frame.activeInHierarchy);
	}
	public string PlacedCardName {
		get { return handPlace.GetChild (0).name;}
	}

	public void SetFrameActive (bool flag) {
		FrameActivity = flag;
	}
	public void DrawFrame () {
		frame.SetActive (FrameActivity);
	}
}