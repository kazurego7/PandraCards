using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandSelector : MonoBehaviour {
    IList<HandPlace> handPlaces;

    void Start () {
        handPlaces = GetComponentsInChildren<HandPlace> ().OrderBy (o => o.name).ToList ();
    }
    public void SelectFrame (int sentPlaceNum) {
        //Debug.Log ($"SelectFrame{sentPlaceNum}");
        var sentPlace = handPlaces[sentPlaceNum];
        if (sentPlace.FrameActivity) {
            sentPlace.SetFrameActive (false);
        } else {
            sentPlace.SetFrameActive (true);
        }
        DrawFrames ();
    }

    public void DeselectFrame () {
        //Debug.Log ("DeselectFrame");
    }
    public void DrawFrames () {
        //Debug.Log ("DrawFrames");
        foreach (var handPlace in handPlaces) {
            handPlace.DrawFrame ();
        }
    }

}