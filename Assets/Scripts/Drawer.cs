using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// Drawableで描画用のストリームに流れてきたイベント(IMsg)の設定と処理を実際に行うクラス

// !!!Drawableでなく、こちらを変更する。
public class Drawer {
	public IObservable<Unit> Draw (IMsg msg) {
		switch (msg) {
			case Play play:
				return Observable.ReturnUnit ();
			default:
				return Observable.ReturnUnit ();
		}
	}
	public interface IMsg { }
	interface Play : IMsg { }
}