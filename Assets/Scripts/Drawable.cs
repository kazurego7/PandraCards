using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 描画用のストリームへのイベントオブザーバブル(IObservable)のコマンド(ReactiveCommand)生成するためのクラス
// イベントオブザーバブルが流れると順次処理される
// 前のイベントの処理が終わる前に次のイベントが流れてくると、処理が終わるまで待機する
public class Drawable {
	public (IReactiveCommand<IObservable<Unit>> syncCmd, IReactiveCommand<IObservable<Unit>> asyncCmd) GetDrawCommand () {
		var syncCommand = new ReactiveCommand<IObservable<Unit>> ();
		var asyncCommand = new ReactiveCommand<IObservable<Unit>> ();
		syncCommand.Concat ().Subscribe (); // 前のイベントの処理が終わる前に次のイベントが流れてくると、処理が終わるまで待機する
		asyncCommand.Merge ().Subscribe (); // 前のイベントに関係なくイベントが流れる
		return (syncCommand, asyncCommand);
	}
}