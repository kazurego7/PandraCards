using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFieldInitializer : MonoBehaviour {
    [SerializeField] FieldPlacer fieldPlacer;

    public void DrawFirstCardPlacing (){
        StartCoroutine(fieldPlacer.DrawFirstCardPlacing());
    }
}
