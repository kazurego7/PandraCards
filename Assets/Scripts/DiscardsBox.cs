using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardsBox : MonoBehaviour {
	IList<IList<IList<Card>> > discardsList;

	public void Awake () {
		discardsList = new List<IList<IList<Card>> > ();
	}

	public void Add(IList<IList<Card>> disCards){
		discardsList.Add(disCards);
	}

	public void Delete(){
		var linerDiscards = discardsList.SelectMany(x => x).SelectMany(x => x);
		foreach (var discard in linerDiscards)
		{
			Destroy(discard.gameObject);
		}
	}

	public IEnumerator DrawRemovePlayAreaCards () {
		var linerDiscards = discardsList.SelectMany (x => x).SelectMany (x => x);
		var startDelaySecond = 0.07f;
		foreach (var discard in linerDiscards) {
			StartCoroutine(discard.DrawMove (transform.position, 15));
			yield return new WaitForSeconds (startDelaySecond);
		}
		yield return null;
	}
}