using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState : MonoBehaviour {
	public bool IsSelected {
		set;
		get;
	}

	public void ShowName () {
		Debug.Log (this.name);
	}
}