using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayArea : MonoBehaviour {
    [SerializeField] OnePlayArea playArea;
    [SerializeField] Field field;
    [SerializeField] UIHandSelector UIHandSelector;
    [SerializeField] Drawable drawable;
    public void PlayCardForHand () {
        drawable.SyncCommand.Execute (field.PlayCardsForHand (UIHandSelector.GetHand (), playArea));
        UIHandSelector.DeselectFrames ();
    }
}