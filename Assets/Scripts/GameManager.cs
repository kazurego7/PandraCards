using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Field field;

	void Start () {
		field.SetUp ();
	}
}