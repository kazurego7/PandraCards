using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPlace : MonoBehaviour {
	public void SetCard(GameObject card){
		transform.SetParent(card.transform);
	}
}
