using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using static UniRx.Observable;

public class Store {

	public IReadOnlyDictionary<OneHand, OneHandView> MyHandPresenter { get; }
	public (Deck model, DeckView view) MyDeckPresenter { get; }
	public IReadOnlyDictionary<OnePlayArea, PlayAreaView> PlayAreaPresenter { get; }
	public (DiscardsBox model, DiscardBoxView view) DiscardBoxPresenter { get; }
	public IReadOnlyDictionary<Card, CardView> CardPresenter { get; }

	public Store () {
		var replenishMessage = Observable.Merge (MyHandPresenter.Keys.Select (model => model.ReplenishedNotice));
		var replenishDraw = replenishMessage.Select (msg => {
			var cardView = CardPresenter[msg.movee];
			var oneHandView = MyHandPresenter[msg.target];
			return OneHandView.Replenish (cardView, oneHandView);
		});
		var shuffleMessage = MyDeckPresenter.model.ShuffledNotice;
		var shuffleDraw = shuffleMessage.Select (msg => {
			var cardViews = msg.Select (card => CardPresenter[card]).ToList ();
			return DeckView.Shuffle (cardViews);
		});
		var playMessage = Observable.Merge (PlayAreaPresenter.Keys.Select (model => model.PlayNotice));
		var playDraw = playMessage.Select (msg => {
			var cardViews = msg.playCards.Select (card => CardPresenter[card]).ToList ();
			var playAreaInfo = (msg.putArea, PlayAreaPresenter[msg.putArea]);
			return PlayAreaView.Play (cardViews, playAreaInfo);
		});
		var firstPutMessage = Observable.Merge (PlayAreaPresenter.Keys.Select (model => model.FirstPutNotice));
		var firstPutDraw = firstPutMessage.Select (msg => {
			var cardView = CardPresenter[msg.firstCard];
			var playAreaView = PlayAreaPresenter[msg.putArea];
			return PlayAreaView.FirstPut (cardView, playAreaView);
		});
		var discardMessage = DiscardBoxPresenter.model.DiscardNotice;
		var discardDraw = discardMessage.Select (msg => {
			var cardViews = msg.discards.SelectMany (cards => cards.Select (card => CardPresenter[card])).ToList ();
			var discardBoxView = DiscardBoxPresenter.view;
			return DiscardBoxView.Discard (cardViews, discardBoxView);
		});

		Merge (
			replenishDraw,
			shuffleDraw,
			playDraw,
			firstPutDraw,
			discardDraw
		).Concat ().Subscribe ();
	}

	public struct OneHandView {
		public static IObservable<Unit> Replenish (CardView cardView, OneHandView oneHandView) {
			var nextPoss = CardView.LinerMoves (source: cardView.Transform.position, target: oneHandView.Transform.position, moveingFrame: 7);
			return nextPoss.Select (nextPos => {
				cardView.Transform.Rotate (nextPos);
				return Unit.Default;
			});
		}
		public Transform Transform { get; }
	}
	public struct DeckView {
		public static IObservable<Unit> Shuffle (IList<CardView> cards) {
			var delayFrame = 1;
			return Range (0, cards.Count).Select (count =>
				Concat (
					TimerFrame (delayFrame).AsUnitObservable (),
					ShuffleOne (cards[(int) count])
				)).Merge ().Concat ();
			IObservable<Unit> ShuffleOne (CardView cardView) {
				// 各カード1枚ごとの処理
				var moveVec = cardView.Transform.right * 3f;
				var start = cardView.Transform.position;
				var middle = start + moveVec;

				var movePos = Concat (
					CardView.LinerMoves (start, middle, moveingFrame : 10),
					CardView.LinerMoves (middle, start, moveingFrame : 10)
				);

				return movePos.Select (nextPos => {
					cardView.Transform.Rotate (nextPos);
					return Unit.Default;
				});
			}
		}
		public Transform Transform { get; }
	}
	public struct PlayAreaView {
		public static IObservable<Unit> Play (IList<CardView> cardViews, (OnePlayArea model, PlayAreaView view) playAreaInfo) {
			var playCardBottomPosZ = (playAreaInfo.model.CountPlayedCards () - cardViews.Count) * Card.thickness * Vector3.back;
			return cardViews.Select ((cardView, i) => {
				var distance = 0.2f;
				var moveXY = distance * (-(cardViews.Count - 1) + 2 * i) * Vector3.left;
				var moveZ = playCardBottomPosZ + Card.thickness * i * Vector3.back;
				var source = cardView.Transform.position;
				var cardMove = Concat (
					CardView.LinerMoves (source, source + moveZ),
					CardView.LinerMoves (source + moveZ, source + moveZ + moveXY)
				);
				return Observable
					.Select (cardMove, nextPos => {
						cardView.Transform.Rotate (nextPos);
						return Unit.Default;
					});
			}).Merge ();
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
		public static IObservable<Vector3> LinerMoves (Vector3 source, Vector3 target, int moveingFrame = 1) {
			return Observable
				.TimerFrame (0, 1)
				.TakeWhile (t => t <= moveingFrame)
				.Select (t => Vector3.Lerp (source, target, (float) t / moveingFrame));
		}
		public Transform Transform { get; }
	}
}