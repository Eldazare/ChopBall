using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentBattleController  {

	private static CurrentBattleStorage currentBattle;

	private static void LoadCurrentBattle(){
		if (currentBattle == null) {
			currentBattle = (CurrentBattleStorage) Resources.Load ("Scriptables/Battle/Storages/CurrentBattleStorage", typeof(CurrentBattleStorage));
		}
	}

	public static void InitializeCurrentData(){
		LoadCurrentBattle ();
		currentBattle.LoadFromMasterState ();
	}

	// "Was this the last goal of the match?"
	public static void AddGoal(GoalData goalData){
		LoadCurrentBattle ();
		Debug.Log ("Goal detected: " + goalData.goalPlayerID + " by " + goalData.giverPlayerID);
	}

	// "Did game end?"
	public static void AdvanceTime(float deltaTime){
		LoadCurrentBattle ();
	}
}
