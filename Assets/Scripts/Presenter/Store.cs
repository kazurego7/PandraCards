using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour {
	public struct OneHandStore {
		public OneHand model;
		public Transform transform;
	}

	public struct CardStore {
		public Card model;
		public Transform transform;
	}

	public struct HandStore {
		public OneHandStore[] handStores;
	}
}