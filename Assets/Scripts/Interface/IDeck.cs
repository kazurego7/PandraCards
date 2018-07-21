using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeck : IShufflable {
	void DrawHeightAdjustedCards ();
	GameObject TopDraw ();
}