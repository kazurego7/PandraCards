using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Text;

public class UICardNumInputer : MonoBehaviour {
	InputField cardNumInput;
	[SerializeField] DeckReciper deckRecier;
	public void Awake () {
		cardNumInput = GetComponent<InputField>();
	}

	public void ChangeRedCardNum(){
		int parsedNum;
		var tryParsed = Int32.TryParse(cardNumInput.text, out parsedNum);
		if (tryParsed) {
			deckRecier.RedNum = parsedNum;
		} else {
			deckRecier.RedNum = 0;
		}
	}

	public void ChangeBlueCardNum(){
		int parsedNum;
		var tryParsed = Int32.TryParse(cardNumInput.text, out parsedNum);
		if (tryParsed) {
			deckRecier.BlueNum = parsedNum;
		} else {
			deckRecier.BlueNum = 0;
		}
	}

	public void ChangeGreenCardNum(){
		int parsedNum;
		var tryParsed = Int32.TryParse(cardNumInput.text, out parsedNum);
		if (tryParsed) {
			deckRecier.GreenNum = parsedNum;
		} else {
			deckRecier.GreenNum = 0;
		}
	}
}
