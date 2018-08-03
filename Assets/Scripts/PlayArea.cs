using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {
	public void PlayCard(GameObject card){
		transform.SetParent(card.transform);
	}
}
