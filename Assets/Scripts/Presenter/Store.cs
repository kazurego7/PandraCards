using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Store {
	public IReadOnlyDictionary<OneHand, OneHandView> MyHandPresenter { get; }
	public (Deck model, DeckView view) MyDeckPresenter { get; }
	public IReadOnlyDictionary<PlayArea, PlayAreaView> PlayAreaPresenter { get; }
	public (DiscardsBox model, DiscardBoxView view) DiscardBoxPresenter { get; }
	public IReadOnlyDictionary<Card, CardView> CardPresenter { get; }
	public IObservable<IMessage> MessageBox { get; }

	public Store () {
		IObservable<IMessage> replenishe = Observable.Merge (MyHandPresenter.Keys.Select (model => model.ReplenishedNotice));
		IObservable<IMessage> shuffle = MyDeckPresenter.model.ShuffledNotice;
		IObservable<IMessage> play = Observable.Merge (PlayAreaPresenter.Keys.Select (model => model.PlayNotice));
		IObservable<IMessage> firstPut = Observable.Merge (PlayAreaPresenter.Keys.Select (model => model.FirstPutNotice));
		IObservable<IMessage> discard = DiscardBoxPresenter.model.DiscardNotice;
		MessageBox = Observable.Merge (replenishe, shuffle, play, firstPut, discard);
	}

	public struct OneHandView {
		public Transform Transform { get; }
	}
	public struct DeckView {
		public Transform Transform { get; }
	}
	public struct PlayAreaView {
		public Transform Transform { get; }
	}

	public struct DiscardBoxView {
		public Transform Transform { get; }
	}
	public struct CardView {
		public Transform Transform { get; }
	}
}