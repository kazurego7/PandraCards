using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFieldInitializer : MonoBehaviour {
    [SerializeField] Field field;

    public void DrawFirstCardPlacing (){
        StartCoroutine(field.DrawFirstCardPlacing());
    }
}
