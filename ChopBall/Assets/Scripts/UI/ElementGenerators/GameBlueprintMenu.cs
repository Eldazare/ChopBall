using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameBlueprintMenu : MonoBehaviour {

	int SECONDSPERCLICK = 15;


	// TODO: Maybe transform logic to masterState?

	public List<BlueprintChangeButton> blueprintButtons;
	BattleModeBlueprint blueprint;
	MasterStateData masterState;

	void Awake(){
		masterState = MasterStateController.GetTheMasterData ();
		blueprint = masterState.battleModeBlueprint;
		if (blueprint == null) {
			MasterStateController.GetTheMasterData ().SetBattleDefaults ();
			blueprint = MasterStateController.GetTheMasterData().battleModeBlueprint;
		}
		SetAllButtons ();
	}

	private void SetAllButtons(){
		// Blueprint value
		/*
		public CountObject countObject;
		public RoundEnd roundEnd;
		public int roundEndCap; // Starting stock or goalCap, per round. Unnecessary with timer.
		public ATime timer;
		public MatchEnd endCriteria; // For ending the match
		public int endValue;
		public ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round
		*/

		blueprintButtons [0].Initialize (IncDecCountObject);
		blueprintButtons [0].SetString (blueprint.countObject.ToString());

		blueprintButtons [1].Initialize (IncDecRoundEnd);
		blueprintButtons [1].SetString (blueprint.roundEnd.ToString());
		if (blueprint.roundEnd == RoundEnd.Timer) {
			blueprintButtons [2].SetString (blueprint.timer.GetAsString ());
		} else {
			blueprintButtons [2].SetString (blueprint.roundEndCap.ToString());
		}

		blueprintButtons [2].Initialize (IncDecRoundEndCapOrTimer);

		blueprintButtons [3].Initialize (IncDecEndCriteria);
		blueprintButtons [3].SetString (blueprint.endCriteria.ToString ());

		blueprintButtons [4].Initialize (IncDecMatchEndValue);
		blueprintButtons [4].SetString (blueprint.endValue.ToString());

		blueprintButtons [5].Initialize (IncDecScoringMode);
		blueprintButtons [5].SetString (blueprint.scoringMode.ToString ());

		blueprintButtons [6].Initialize (IncDecGrandMode);
		blueprintButtons [6].SetString (masterState.mode.ToString ());
	}

	private int CheckIndex(int currentIndex, int length, bool incDec){
		if (incDec) {
			currentIndex += 1;
		} else {
			currentIndex -= 1;
		}
		if (currentIndex >= length) {
			return 0;
		} else if (currentIndex < 0) {
			return length - 1;
		}
		return currentIndex;
	}

	private void IncDecCountObject(bool incDec){
		if (!masterState.teams) {
			int nextIndex = CheckIndex ((int)blueprint.countObject, Enum.GetValues (typeof(CountObject)).Length, incDec);
			blueprint.countObject = (CountObject)nextIndex;
			blueprintButtons [0].SetString (blueprint.countObject.ToString ());
		}
	}

	private void IncDecRoundEnd(bool incDec){
		int nextIndex = CheckIndex ((int)blueprint.roundEnd, Enum.GetValues (typeof(RoundEnd)).Length, incDec);
		blueprint.roundEnd = (RoundEnd)nextIndex;
		blueprintButtons [1].SetString (blueprint.roundEnd.ToString());
		if (blueprint.roundEnd == RoundEnd.Timer) {
			blueprintButtons [2].SetString (blueprint.timer.GetAsString ());
		} else {
			blueprintButtons [2].SetString (blueprint.roundEndCap.ToString());
		}
	}

	private void IncDecRoundEndCapOrTimer(bool incDec){
		if (blueprint.roundEnd == RoundEnd.Timer) {
			int nextIndex = CheckIndex (blueprint.timer.GetScale(SECONDSPERCLICK), 99*60/SECONDSPERCLICK, incDec);
			blueprint.timer.SetScale (nextIndex, SECONDSPERCLICK);
			blueprintButtons [2].SetString (blueprint.timer.GetAsString ());
		} else {
			int nextIndex = CheckIndex (blueprint.roundEndCap, 100, incDec);
			blueprint.roundEndCap = nextIndex;
			blueprintButtons [2].SetString (blueprint.roundEndCap.ToString());
		}
	}

	private void IncDecEndCriteria(bool incDec){
		int nextIndex = CheckIndex ((int)blueprint.endCriteria, Enum.GetValues (typeof(MatchEnd)).Length, incDec);
		blueprint.endCriteria = (MatchEnd)nextIndex;
		blueprintButtons [3].SetString (blueprint.endCriteria.ToString ());
	}

	private void IncDecMatchEndValue(bool incDec){
		int nextIndex = CheckIndex (blueprint.endValue, 999, incDec);
		blueprint.endValue = nextIndex;
		blueprintButtons [4].SetString (blueprint.endValue.ToString());
	}

	private void IncDecScoringMode(bool incDec){
		int nextIndex = CheckIndex ((int)blueprint.scoringMode, Enum.GetValues (typeof(ScoringMode)).Length, incDec);
		blueprint.scoringMode = (ScoringMode)nextIndex;
		blueprintButtons [5].SetString (blueprint.scoringMode.ToString ());
	}

	private void IncDecGrandMode(bool incDec){
		int nextIndex = CheckIndex ((int)masterState.mode, Enum.GetValues (typeof(GrandMode)).Length, incDec);
		masterState.SetGrandMode ((GrandMode)nextIndex);
		blueprintButtons [6].SetString (masterState.mode.ToString ());
		blueprintButtons [0].SetString (blueprint.countObject.ToString ());
	}

	void OnDisable(){
		SaveBlueprint ();
	}

	public void SaveBlueprint(){
		MasterStateController.GetTheMasterData ().SetBattleModeBlueprint (blueprint);
	}
}
