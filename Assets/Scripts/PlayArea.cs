using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	public void PlayCard(GameObject card){
		card.transform.SetParent(transform);
	}
	
}
