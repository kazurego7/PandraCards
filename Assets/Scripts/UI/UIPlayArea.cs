using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayArea : MonoBehaviour {
    [SerializeField] PlayArea playArea;
    [SerializeField] Field field;
    [SerializeField] UIHandSelector UIHandSelector;
    public void PlayCardForHand(){
        field.PlayCardsForHand(UIHandSelector.GetHand(), playArea);
        UIHandSelector.DeselectFrames();
    }
}
