using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IOOnePlayArea : MonoBehaviour {
	OnePlayArea onePlayArea;
	GameManager gameManager;
	Drawable drawable;
	IList<Deck> decks;
	IList<Hand> hands;
	IList<OnePlayArea> onePlayAreas;
	DiscardsBox discardBox;

	void Awake () {
		onePlayArea = GetComponent<OnePlayArea> ();
		gameManager = GetComponentInParent<GameManager> ();
		drawable = gameManager.GetComponent<Drawable> ();
		decks = gameManager.GetComponentsInChildren<Deck> ();
		hands = gameManager.GetComponentsInChildren<Hand> ();
		onePlayAreas = gameManager.GetComponentsInChildren<OnePlayArea> ();
		discardBox = gameManager.GetComponentInChildren<DiscardsBox> ();
	}

	void Start () {
		this.OnMouseDownAsObservable ()
			.Subscribe (_ => {
				// 	カードプレイ＆補充
				var canNotPlay = !onePlayArea.CanPlay (hands[0].GetSelectedCards ());
				if (canNotPlay) return;
				onePlayArea.Play (hands[0].RemoveSelectedCards ());
				hands[0].Deal (decks[0]);

				var drawPlay = onePlayArea.DrawPlay ();
				var drawHandDeal = hands[0].DrawDeal ();
				drawable.SyncCommand.Execute (drawPlay.Merge (drawHandDeal));

				// 次プレイできなければ、再配置処理
				var canNextPlay = hands
					.Any (hand => onePlayAreas
						.Any (onePlayArea => onePlayArea.CanNextPlay (hand)));
				if (canNextPlay) return;
				foreach (var hand in hands) {
					hand.Deal (decks[0]);
				}

				foreach (var onePlayArea in onePlayAreas) {
					onePlayArea.Deal (decks[0].TopDraw ());
				}

				var drawRemoveForPlayArea = Observable.TimerFrame (120).AsUnitObservable ()
					.Concat (discardBox.DrawRemoveForPlayArea ());
				var drawPlayAreaDeal = onePlayAreas.Select (onePlayArea => onePlayArea.DrawDeal ()).Merge ();
				drawable.SyncCommand.Execute (drawRemoveForPlayArea.Concat (drawPlayAreaDeal));
			});
	}
}