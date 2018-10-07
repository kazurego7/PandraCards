using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Store {

	IReadOnlyDictionary<OneHand, OneHandView> MyHandPresenter { get; }
		(Deck model, DeckView view) MyDeckPresenter { get; }
	IReadOnlyDictionary<PlayArea, PlayAreaView> PlayAreaPresenter { get; }
		(DiscardsBox model, DiscardBoxView view) DiscardBoxPresenter { get; }
	IReadOnlyDictionary<Card, CardView> CardPresenter { get; }

	public Store () {
		var replenishMessage = Observable.Merge (MyHandPresenter.Keys.Select (model => model.ReplenishedNotice));
		var replenishDraw = replenishMessage
			.Select (msg => {
				var cardView = CardPresenter[msg.mover];
				var oneHandView = MyHandPresenter[msg.target];
				return OneHandView.Replenish (cardView, oneHandView);
			});
		var shuffleMessage = MyDeckPresenter.model.ShuffledNotice;
		var shuffleDraw = shuffleMessage
			.Select (msg => {
				var cardViews = msg.Select (card => CardPresenter[card]).ToList ();
				return DeckView.Shuffle (cardViews);
			});
		var playMessage = Observable.Merge (PlayAreaPresenter.Keys.Select (model => model.PlayNotice));
		var playDraw = playMessage
			.Select (msg => {
				var cardViews = msg.playCards.Select (card => CardPresenter[card]).ToList ();
				var playAreaView = PlayAreaPresenter[msg.putArea];
				return PlayAreaView.Play (cardViews, playAreaView);
			});
		var firstPutMessage = Observable.Merge (PlayAreaPresenter.Keys.Select (model => model.FirstPutNotice));
		var firstPutDraw = firstPutMessage
			.Select (msg => {
				var cardView = CardPresenter[msg.firstCard];
				var playAreaView = PlayAreaPresenter[msg.putArea];
				return PlayAreaView.FirstPut (cardView, playAreaView);
			});
		var discardMessage = DiscardBoxPresenter.model.DiscardNotice;
		var discardDraw = discardMessage
			.Select (msg => {
				var cardViews = msg.discards.SelectMany (cards => cards.Select (card => CardPresenter[card])).ToList ();
				var discardBoxView = DiscardBoxPresenter.view;
				return DiscardBoxView.Discard (cardViews, discardBoxView);
			});
	}

	public struct OneHandView {
		public static IObservable<Unit> Replenish (CardView cardView, OneHandView oneHandView) {
			return Observable.ReturnUnit ();
		}
		public Transform Transform { get; }
	}
	public struct DeckView {
		public static IObservable<Unit> Shuffle (IList<CardView> cards) {
			return Observable.ReturnUnit ();
		}
		public Transform Transform { get; }
	}
	public struct PlayAreaView {
		public static IObservable<Unit> Play (IList<CardView> cardViews, PlayAreaView playAreaView) {
			return Observable.ReturnUnit ();
		}
		public static IObservable<Unit> FirstPut (CardView cardView, PlayAreaView playAreaView) {
			return Observable.ReturnUnit ();
		}
		public Transform Transform { get; }
	}

	public struct DiscardBoxView {
		public static IObservable<Unit> Discard (IList<CardView> cardViews, DiscardBoxView discardBoxView) {
			return Observable.ReturnUnit ();
		}
		public Transform Transform { get; }
	}
	public struct CardView {
		public Transform Transform { get; }
	}
}