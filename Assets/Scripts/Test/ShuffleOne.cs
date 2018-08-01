using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleOne : MonoBehaviour {
	void Awake () {
		var button = GetComponent<Button> ()
			.OnPointerClickAsObservable ()
			.ThrottleFirstFrame(60)
			.Subscribe (l => Debug.Log ("on"))
			.AddTo(this);
	}
}