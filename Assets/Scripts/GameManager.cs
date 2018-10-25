using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Field field;

	public IReactiveCommand<Drawer.IMsg> DrawCommand;
	void Start () {
		DrawCommand = new Drawable ().GetDrawCommand ();
		field.SetUp ();
		StartCoroutine (field.DrawFirstCardPlacing ());
	}
}