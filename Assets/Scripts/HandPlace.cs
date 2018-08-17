﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HandPlace : MonoBehaviour {

	public Card PlacedCard{
		get;
		set;
	}

	public Card RemoveCard() {
		var removed = PlacedCard;
		PlacedCard = null;
		return removed;
	}
}
