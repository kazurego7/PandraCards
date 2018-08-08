using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HandPlace : MonoBehaviour {
	
	public Transform GetCard() {
		return transform.GetChild(0);
	}
	public void SetCard(Card card){
		card.transform.SetParent(transform);
	}
}
