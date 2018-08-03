using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HandPlace : MonoBehaviour {
	
	public Transform GetCard() {
		if (transform.childCount < 1){
			return null;
		} else {
			return transform.GetChild(0);
		}
	}
	public void SetCard(GameObject card){
		transform.SetParent(card.transform);
	}
}
