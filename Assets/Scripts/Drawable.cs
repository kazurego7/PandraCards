using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 描画用のストリームへのイベント(IMsg)のコマンド(ReactiveCommand)生成するためのクラス
// コマンドにイベントが流れてくると、Drawerによって処理される
// Drawerの処理が終わる前に次のイベントが流れてくると、処理が終わるまで待機する

// !!!基本的に変更しない
public class Drawable {
	public IReactiveCommand<Drawer.IMsg> GetDrawCommand () {
		var command = new BoolReactiveProperty (true).ToReactiveCommand<Drawer.IMsg> ();
		var drawer = new Drawer ();
		command.Select (msg => drawer.Draw (msg)).Merge ().Concat ().Subscribe (); // Drawerの処理が終わる前に次のイベントが流れてくると、処理が終わるまで待機する
		return command;
	}
}