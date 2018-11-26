using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFieldInitializer : MonoBehaviour {
    [SerializeField] Field field;
    GameManager gameManager;

    public void DrawFirstCardPlacing () {
        //gameManager.SyncDrawCommand.Execute (field.DrawFirstCardPlacing ());
    }
}