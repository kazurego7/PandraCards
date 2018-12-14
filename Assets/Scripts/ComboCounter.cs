using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour {
	public GameObject elapsedTimer;
	public GameObject comboTimer;

	float _comboTimeRate = 0; // 0~1まで
	int _comboCount = 0;

	void Update () {
		// 処理
		if (_comboTimeRate <= 0) {
			_comboTimeRate = 0;
			_comboCount = 0;
		} else {
			var reductionForSecondRate = 0.3f;
			_comboTimeRate = _comboTimeRate - reductionForSecondRate * Time.deltaTime;
		}

		// 描画
		var elapsedTimerRectTransform = elapsedTimer.GetComponent<RectTransform> ();
		var comboTimerWidth = comboTimer.GetComponent<RectTransform> ().sizeDelta.x;
		elapsedTimerRectTransform.sizeDelta = new Vector2 (comboTimerWidth * _comboTimeRate, elapsedTimerRectTransform.sizeDelta.y);
		var comboCounterText = GetComponent<Text> ();
		comboCounterText.text = _comboCount.ToString ();

	}

	public void Combo () {
		_comboCount = _comboCount + 1;
		_comboTimeRate = 1;
	}
}