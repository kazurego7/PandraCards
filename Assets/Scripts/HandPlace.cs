using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HandPlace : MonoBehaviour {
	public Card GetCard() {
		return transform.GetChild(0).GetComponent<Card>();
	}
	public void SetCard(Card card){
		card.transform.SetParent(transform);
	}

}
