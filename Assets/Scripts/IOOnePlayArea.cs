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
	IList<ComboCounter> comboCounters;
	DiscardsBox discardBox;
	int playerNumber = 0;

	void Awake () {
		onePlayArea = GetComponent<OnePlayArea> ();
		gameManager = GetComponentInParent<GameManager> ();
		drawable = gameManager.GetComponent<Drawable> ();
		decks = gameManager.GetComponentsInChildren<Deck> ();
		hands = gameManager.GetComponentsInChildren<Hand> ();
		onePlayAreas = gameManager.GetComponentsInChildren<OnePlayArea> ();
		comboCounters = gameManager.GetComponentsInChildren<ComboCounter> ();
		discardBox = gameManager.GetComponentInChildren<DiscardsBox> ();
	}

	void Start () {
		this.OnMouseDownAsObservable ()
			.Subscribe (_ => {
				// 	カードプレイ＆補充
				var canCurrentPlay = onePlayArea.CanPlay (hands[0].GetSelectedCards ());
				if (!canCurrentPlay) return;
				onePlayArea.Play (hands[playerNumber].RemoveSelectedCards ());
				hands[playerNumber].Deal (decks[playerNumber]);
				hands[playerNumber].DecelectFrame ();

				var drawPlay = onePlayArea.DrawPlay ();
				var drawHandDeal = hands[playerNumber].DrawDeal ();
				var drawFrame = hands[playerNumber].DrawFrame ();
				drawable.SyncCommand.Execute (drawPlay.Merge (drawHandDeal).Merge (drawFrame));

				
				// 次プレイできなければ、再配置処理
				var canNextPlay = hands // hands × onePlayAreas で CanNextPlay
					.Any (hand => onePlayAreas
						.Any (onePlayArea => onePlayArea.CanNextPlay (hand)));
				if (canNextPlay) return;

				Debug.Log ("cannotPlay!");
				foreach (var onePlayArea in onePlayAreas) {
					discardBox.Store (onePlayArea.RemoveAll ());
				}

				foreach (var onePlayArea in onePlayAreas) {
					onePlayArea.Deal (decks[playerNumber].TopDraw ());
				}

				var drawRemoveForPlayArea = Observable.TimerFrame (120).AsUnitObservable ()
					.Concat (discardBox.DrawRemoveForPlayArea ());
				var drawPlayAreaDeal = onePlayAreas.Select (onePlayArea => onePlayArea.DrawDeal ()).Merge ();
				//drawable.SyncCommand.Execute (Observable.ReturnUnit ().ForEachAsync (a => Debug.Log ("ok")));
				drawable.SyncCommand.Execute (drawRemoveForPlayArea.Concat (drawPlayAreaDeal));
			});
	}
}