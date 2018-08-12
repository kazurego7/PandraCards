using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour {
	public enum Color { Red, Green, Blue, NoColor };
 	const float thickness = 0.0005f; // 遊戯王カードの厚みが0.5mm = 0.0005m
	public float Thickness {
		get {return thickness;}
	}
	[SerializeField] Color _myColor;
	public Color MyColor {
		get {
			return _myColor;
		}
	}

	public IEnumerator DrawMove (Vector3 target, float height = thickness, int moveingFrame = 1) {
		var start = transform.position;
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			transform.position = Vector3.Lerp (start, target, (float) currentFrame / moveingFrame);
			yield return null;
		}
	}

	public IEnumerator DrawShuffle () {

		// 設定項目
		var moveingFrame = 10;
		var moveVec = transform.right * 3f;

		var start = transform.position;
		var middle = start + moveVec;
		// middleまでmoveingFrameで動かす
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			transform.position = Vector3.Lerp (start, middle, (float) currentFrame / moveingFrame);
			yield return null;
		}
		// startまでmoveingFrameで動かす
		foreach (var currentFrame in Enumerable.Range (1, moveingFrame)) {
			transform.position = Vector3.Lerp (middle, start, (float) currentFrame / moveingFrame);
			yield return null;
		}
	}
}