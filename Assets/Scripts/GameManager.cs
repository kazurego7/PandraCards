using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Field field;

	public IReactiveCommand<IObservable<Unit>> SyncDrawCommand {
		get;
		private set;
	}
	public IReactiveCommand<IObservable<Unit>> AsyncDrawCommand {
		get;
		private set;
	}
	void Start () {
		(SyncDrawCommand, AsyncDrawCommand) = new Drawable ().GetDrawCommand ();
		field.SetUp ();
		SyncDrawCommand.Execute (field.DrawFirstCardPlacing ());
	}
}