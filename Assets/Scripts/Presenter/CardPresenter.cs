using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class CardPresenter : MonoBehaviour {

	static readonly DeckPresenter deckPresenter;
	static readonly OneHandPresenter[] oneHandPresenters;
	static readonly PlayAreaPresenter[] playAreaPresenters;
	static readonly DiscardBoxPresenter discardBoxPresenter;
}