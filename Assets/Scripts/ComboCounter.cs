using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour {
	public GameObject elapsedTimer;
	public GameObject comboTimer;

	float comboTimeRate = 0; // 0~1まで
	int comboCount = 0;

	void Start () {
		Debug.Log (comboTimer.GetComponent<RectTransform> ().sizeDelta.x);
	}

	void Update () {
		// 処理
		if (comboTimeRate <= 0) {
			comboTimeRate = 0;
			comboCount = 0;
		} else {
			var reductionForSecondRate = 0.3f;
			comboTimeRate = comboTimeRate - reductionForSecondRate * Time.deltaTime;
		}

		// 描画
		var elapsedTimerRectTransform = elapsedTimer.GetComponent<RectTransform> ();
		var comboTimerWidth = comboTimer.GetComponent<RectTransform> ().sizeDelta.x;
		elapsedTimerRectTransform.sizeDelta = new Vector2 (comboTimerWidth * comboTimeRate, elapsedTimerRectTransform.sizeDelta.y);
		Debug.Log (elapsedTimerRectTransform.sizeDelta.x);
		var comboCounterText = GetComponent<Text> ().text;
		comboCounterText = comboCount.ToString ();

	}

	public void Combo () {
		comboCount = comboCount + 1;
		comboTimeRate = 1;
	}
}