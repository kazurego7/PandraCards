using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleDeckButton : MonoBehaviour {
	[SerializeField] Deck deck;

	public void OnClick () {
		StartCoroutine (deck.ShuffleDraw ());
	}
}