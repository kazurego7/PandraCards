using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandSelector : MonoBehaviour {
    IList<HandPlace> handPlaces;

    void Awake () {
        handPlaces = GetComponentsInChildren<HandPlace> ().OrderBy (o => o.name).ToList ();
    }
    public void SelectFrame (int sentPlaceNum) {
        //Debug.Log ($"SelectFrame{sentPlaceNum}");

        var sentPlace = handPlaces[sentPlaceNum];
        var isSamePlacedCard = handPlaces.Where (place => place.FrameActivity).All (place => place.PlacedCardName == sentPlace.PlacedCardName);
        if (sentPlace.FrameActivity) {
            sentPlace.SetFrameActive (false);
        } else if (isSamePlacedCard) {
            sentPlace.SetFrameActive (true);
        } else {
            foreach (var place in handPlaces) {
                place.SetFrameActive (false);
            }
            sentPlace.SetFrameActive (true);
        }
        DrawFrames ();
    }

    public void DeselectFrame () {
        //Debug.Log ("DeselectFrame");
        foreach (var place in handPlaces) {
            place.SetFrameActive (false);
        }
        DrawFrames ();
    }
    public void DrawFrames () {
        //Debug.Log ("DrawFrames");
        foreach (var handPlace in handPlaces) {
            handPlace.DrawFrame ();
        }
    }

}