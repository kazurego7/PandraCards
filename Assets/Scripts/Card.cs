using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Card : MonoBehaviour {
	public enum Color { Red, Green, Blue, NoColor }
	public const float thickness = 0.0005f; // 遊戯王カードの厚みが0.5mm = 0.0005m
	[SerializeField] Color _myColor;
	public Color MyColor {
		get {
			return _myColor;
		}
	}

	public IObservable<Unit> DrawMove (Vector3 target, int moveingFrame = 1) {
		var start = transform.position;
		return Observable
			.IntervalFrame (1)
			.Take (moveingFrame)
			.ForEachAsync (currentFrame =>
				transform.position = Vector3.Lerp (start, target, (float) (currentFrame + 1) / moveingFrame));
	}

	public IObservable<Unit> DrawShuffle () {
		// 設定項目
		var moveingFrame = 10;
		var moveVec = transform.right * 3f;

		var start = transform.position;
		var middle = start + moveVec;
		var movingFrameTimer = Observable.IntervalFrame (1).Take (moveingFrame);
		var startToMiddle = movingFrameTimer
			.ForEachAsync (currentFrame =>
				transform.position = (Vector3.Lerp (start, middle, (float) (currentFrame + 1) / moveingFrame)));
		var middleToEnd = movingFrameTimer
			.ForEachAsync (currentFrame =>
				transform.position = (Vector3.Lerp (middle, start, (float) (currentFrame + 1) / moveingFrame)));
		return startToMiddle.Concat (middleToEnd);
	}
}