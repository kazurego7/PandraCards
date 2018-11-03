using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 描画用のストリームへのイベントオブザーバブル(IObservable)のコマンド(ReactiveCommand)生成するためのクラス
// イベントオブザーバブルが流れると順次処理される
// 前のイベントの処理が終わる前に次のイベントが流れてくると、処理が終わるまで待機する
public class Drawable : MonoBehaviour {

	public IReactiveCommand<IObservable<Unit>> SyncCommand {
		get;
		private set;
	} = new ReactiveCommand<IObservable<Unit>> ();

	public IReactiveCommand<IObservable<Unit>> AsyncCommand {
		get;
		private set;
	} = new ReactiveCommand<IObservable<Unit>> ();

	void Awake () {
		SyncCommand.Concat ().Subscribe (); // 前のイベントの処理が終わる前に次のイベントが流れてくると、処理が終わるまで待機する
		AsyncCommand.Merge ().Subscribe (); // 前のイベントに関係なくイベントが流れる
	}
}