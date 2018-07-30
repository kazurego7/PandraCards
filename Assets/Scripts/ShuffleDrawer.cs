using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ShuffleDrawer : MonoBehaviour {
    IObservable<Vector3> shufflePipe;
    [SerializeField] int moveingFrame = 100;
    void Awake () {
        // 定義
        var moveVec = transform.right * 3f;
        var start = transform.position;
        var middle = start + moveVec;
        var goShuffle = Observable
            .IntervalFrame (1)
            .Take (moveingFrame)
            .Select (frame => Vector3.Lerp (start, middle, (float) frame / moveingFrame));

        var comebackShuffle = Observable
            .IntervalFrame (1)
            .Take (moveingFrame)
            .Select (frame => Vector3.Lerp (middle, start, (float) frame / moveingFrame));

        shufflePipe = goShuffle.Concat (comebackShuffle);
    }

    public void DrawKthCardShuffle () {
        shufflePipe
            .Subscribe (pos => transform.position = pos)
            .AddTo (this);
    }
}