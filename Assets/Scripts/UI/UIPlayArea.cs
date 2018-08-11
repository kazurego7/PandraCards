using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayArea : MonoBehaviour {
    [SerializeField] PlayArea playArea;
    [SerializeField] UIHandSelector UIHandSelector;
    public void PlayCardForHands(){
        var selectedCards = UIHandSelector.GetSelectedCards();
        foreach (var item in selectedCards)
        {
          Debug.Log(item);  
        }
        if (playArea.CanPlayCards(selectedCards)){
            playArea.PlayCards(selectedCards);
        }


        foreach (var selectedCard in selectedCards)
        {
            StartCoroutine(selectedCard.DrawMove(playArea.transform.position));
        }
    }
}
