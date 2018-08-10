using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHandSelector : MonoBehaviour {
    IList<UIHandPlace> handPlaces;

    void Awake () {
        handPlaces = GetComponentsInChildren<UIHandPlace> ().OrderBy (o => o.name).ToList ();
    }
    public void SelectFrame (UIHandPlace selectedPlace) {
        //Debug.Log ($"SelectFrame{sentPlaceNum}");

        var isSameColorCard = handPlaces.Where (place => place.FrameActivity).All (place => place.PlacedCardName == selectedPlace.PlacedCardName);
        if (selectedPlace.FrameActivity) {
            selectedPlace.SetFrameActive (false);
        } else if (isSameColorCard) {
            selectedPlace.SetFrameActive (true);
        } else {
            foreach (var place in handPlaces) {
                place.SetFrameActive (false);
            }
            selectedPlace.SetFrameActive (true);
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