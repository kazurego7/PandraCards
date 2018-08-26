using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayArea : MonoBehaviour {
    [SerializeField] PlayArea playArea;
    [SerializeField] FieldPlacer fieldPlacer;
    [SerializeField] UIHandSelector UIHandSelector;
    public void PlayCardForHands(){
        fieldPlacer.PlayCardsForHands(UIHandSelector.GetHand(), playArea);
        UIHandSelector.DeselectFrames();
    }
}
