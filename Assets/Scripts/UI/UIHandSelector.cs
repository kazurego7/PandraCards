using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHandSelector : MonoBehaviour {
    IList<UIHandPlace> UIHandPlaces;
    [SerializeField] Hand hand;

    void Awake () {
        UIHandPlaces = GetComponentsInChildren<UIHandPlace> ().ToList ();
    }
    public void SelectFrame (UIHandPlace selectedPlace) {
        //Debug.Log ($"SelectFrame{selectedPlace.name}");

        var selectedColor = selectedPlace.OneHnad.PutCard?.MyColor ?? Card.Color.NoColor;
        bool IsSameColorToSelected (UIHandPlace place) {
            var placedColor = place.OneHnad.PutCard?.MyColor ?? Card.Color.NoColor;
            return placedColor != Card.Color.NoColor && selectedColor != Card.Color.NoColor && placedColor == selectedColor;
        }

        var isSameColorCard = UIHandPlaces.Where (place => place.FrameActivity).All (IsSameColorToSelected);
        if (selectedPlace.FrameActivity || selectedColor == Card.Color.NoColor) {
            selectedPlace.SetFrameActive (false);
        } else if (isSameColorCard) {
            selectedPlace.SetFrameActive (true);
        } else {
            foreach (var place in UIHandPlaces) {
                place.SetFrameActive (false);
            }
            selectedPlace.SetFrameActive (true);
        }
    }

    public void DeselectFrames () {
        // 処理
        foreach (var place in UIHandPlaces) {
            place.SetFrameActive (false);
        }

        // 描画
        DrawFrames ();
    }
    public void DrawFrames () {
        foreach (var handPlace in UIHandPlaces) {
            handPlace.DrawFrame ();
        }
    }

    public Hand GetHand () {
        return hand;
    }
}