using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleDrawer : MonoBehaviour {
    IObservable<Vector3> shuffleMoveing;

    void Awake () {
        IObservable<Vector3> CreateShuffleMoveing (Vector3 start, Vector3 target, int moveingFrame) {
            var goShuffle = Observable
                .IntervalFrame (1)
                .Take (moveingFrame)
                .Select (frame => Vector3.Lerp (start, target, (float) frame / moveingFrame));

            var comebackShuffle = Observable
                .IntervalFrame (1)
                .Take (moveingFrame)
                .Select (frame => Vector3.Lerp (target, start, (float) frame / moveingFrame));

            return goShuffle
                .Concat (comebackShuffle);
        }
        shuffleMoveing = CreateShuffleMoveing (transform.position, transform.position + transform.right * 3f, 100);
    }

    public void DrawCardShuffle () {
        shuffleMoveing
            .Subscribe (pos => transform.position = pos);
    }
}