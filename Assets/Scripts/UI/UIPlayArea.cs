﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayArea : MonoBehaviour {
    [SerializeField] OnePlayArea playArea;
    [SerializeField] Field field;
    [SerializeField] UIHandSelector UIHandSelector;
    public void PlayCardForHand () {
        field.PlayForHand (UIHandSelector.GetHand (), playArea);
        UIHandSelector.DeselectFrames ();
    }
}