using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayArea : MonoBehaviour {
    [SerializeField] PlayArea playArea;
    [SerializeField] UIHandSelector UIHandSelector;
    public void PlayCardForHands(){
        playArea.PlayCardsForHands(UIHandSelector.GetSelectedHandPlaces(), UIHandSelector.GetCardPlacer());
        UIHandSelector.DeselectFrames();
    }
}
