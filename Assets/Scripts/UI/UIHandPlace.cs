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
	public Card.Color PlacedCardName {
		get { return handPlace.GetComponent<HandPlace>().GetCard().MyColor;}
	}

	public void SetFrameActive (bool flag) {
		FrameActivity = flag;
	}
	public void DrawFrame () {
		frame.SetActive (FrameActivity);
	}
}