using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CardPutPresenter {

	ReactiveCollection<IList<Card>> PlayCardsNotice {
		get;
		set;
	}

	ReactiveCollection<Card> DeckNotice {
		get;
		set;
	}

	ReactiveCollection<IList<Card>> HandCardsNotice {
		get;
		set;
	}

	public ReactiveProperty<Vector3> Position {
		get;
	} = new ReactiveProperty<Vector3>(
		Observable.
	)
}
