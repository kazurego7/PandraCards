using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHandSelector : MonoBehaviour {
    IList<UIHandPlace> UIHandPlaces;

    void Awake () {
        UIHandPlaces = GetComponentsInChildren<UIHandPlace> ().OrderBy (o => o.name).ToList ();
    }
    public void SelectFrame (UIHandPlace selectedPlace) {
        //Debug.Log ($"SelectFrame{sentPlaceNum}");

        var isSameColorCard = UIHandPlaces.Where (place => place.FrameActivity).All (place => place.PlacedCard.MyColor == selectedPlace.PlacedCard.MyColor);
        if (selectedPlace.FrameActivity) {
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

    public void DeselectFrame () {
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

    public IList<Card> GetSelectedCards(){
        return UIHandPlaces.Where(UIHandPlace => UIHandPlace.FrameActivity).Select(UIHandPlace => UIHandPlace.PlacedCard).ToList();
    }
}