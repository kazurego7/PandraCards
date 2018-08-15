using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHandSelector : MonoBehaviour {
    IList<UIHandPlace> UIHandPlaces;
    [SerializeField] CardPlacer cardPlacer;

    void Awake () {
        UIHandPlaces = GetComponentsInChildren<UIHandPlace> ().OrderBy (o => o.name).ToList ();
    }
    public void SelectFrame (UIHandPlace selectedPlace) {
        //Debug.Log ($"SelectFrame{sentPlaceNum}");

        
        var selectedColor = selectedPlace.HandPlace.PlacedCard?.MyColor ?? Card.Color.NoColor;
        bool IsSameColorToSelected(UIHandPlace place) {
            var placedColor = place.HandPlace.PlacedCard?.MyColor ?? Card.Color.NoColor;
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

    public IList<HandPlace> GetSelectedHandPlaces(){
        return UIHandPlaces.Where(UIHandPlace => UIHandPlace.FrameActivity).Select(UIHandPlace => UIHandPlace.HandPlace).ToList();
    }

    public CardPlacer GetCardPlacer() {
        return cardPlacer;
    }
}