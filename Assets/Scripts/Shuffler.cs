using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shuffler : MonoBehaviour {

	// Use this for initialization
	void Start () { }

	public IEnumerator ViewShuffle () {
		var moveFrame = 20;
		var start = transform.parent.position;
		var moveVec = transform.right * 3f;
		var middle = start + moveVec;
		foreach (var frame in Enumerable.Range (1, moveFrame)) {
			transform.position = Vector3.Lerp (start, middle, (float) frame / moveFrame);
			yield return null;
		}
		foreach (var frame in Enumerable.Range (1, moveFrame)) {
			transform.position = Vector3.Lerp (middle, start, (float) frame / moveFrame);
			yield return null;
		}
	}

	// Update is called once per frame
	void Update () { }
}