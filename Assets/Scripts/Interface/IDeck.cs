using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeck {
	void Initialize ();
	void DrawHeightAdjustedCards ();
	GameObject TopDraw ();
	void Shuffle ();
	IEnumerator DrawShuffle ();
}