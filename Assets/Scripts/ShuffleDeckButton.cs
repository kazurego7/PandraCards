using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleDeckButton : MonoBehaviour {
	Deck deck;
	// Use this for initialization
	void Start () { }

	// Update is called once per frame
	void Update () {

	}

	public void OneClick () {
		foreach (IShufflable deck in FindObjectsOfType<Deck> ()) {
			deck.DrawShuffle ();
		}
	}
}