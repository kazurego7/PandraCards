using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Field field;
	void Start () {
		field.SetUp();
		StartCoroutine(field.DrawFirstCardPlacing ());
	}
}
