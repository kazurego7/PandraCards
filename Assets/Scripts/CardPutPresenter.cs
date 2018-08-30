using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class CardPutPresenter {

	IList<IReactiveCollection<IList<Card>>> PlayCardsNotices {
		get;
		set;
	} = new List<IReactiveCollection<IList<Card>>>();

	IList<IReactiveCollection<Card>> DeckCardNotices {
		get;
		set;
	} = new List<IReactiveCollection<Card>>();

	IList<IReactiveCollection<IList<Card>>> HandCardsNotices {
		get;
		set;
	} = new List<IReactiveCollection<IList<Card>>>();

	public ReactiveProperty<Vector3> Position {
		get;
	} = new ReactiveProperty<Vector3>();

	CardPutPresenter() {
		(IObservable<T> addObservable, IObservable<T> removeObservable) RxCollections2AddAndRemoveObservables<T> (IList<IReactiveCollection<T>> rxCollections) {
			return rxCollections
			.Select(notice => (notice.ObserveAdd().Select(x => x.Value), notice.ObserveRemove().Select(x => x.Value))).Aggregate((accm, x) => (accm.Item1.Merge(x.Item1), accm.Item2.Merge(x.Item2)));
		}
		var deckAddAndRemoveObservable = RxCollections2AddAndRemoveObservables(DeckCardNotices);
		var handAddAndRemoveObservable = RxCollections2AddAndRemoveObservables(HandCardsNotices);
		var playCardsAddAndRemoveObservable = RxCollections2AddAndRemoveObservables(PlayCardsNotices);
	}
}
