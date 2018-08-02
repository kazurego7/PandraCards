using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldPlacer : MonoBehaviour {
	IList<CardPlacer> cardPlacers;
	[SerializeField] Transform playAreas;

	void Awake(){
		cardPlacers = GetComponentsInChildren<CardPlacer> ().OrderBy(o=>o.name).ToList();
	}
	
	public void Initialize() {
		foreach (var cardPlacer in cardPlacers)
		{
			cardPlacer.Initialize();
		}
	}

	public void DrawFirstCardPlacing() {
		foreach (var cardPlacer in cardPlacers)
		{
			StartCoroutine(cardPlacer.DrawFirstCardPlacing());
		}
	}

}