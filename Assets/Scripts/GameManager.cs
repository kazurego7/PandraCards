using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] FieldPlacer fieldPlacer;
	void Start () {
		fieldPlacer.Initialize();
		StartCoroutine(fieldPlacer.DrawFirstCardPlacing ());
	}
}
