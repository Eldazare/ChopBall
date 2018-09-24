using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentBattleController  {

	private static BattleMode currentBattle;

	private static void LoadCurrentBattle(){
		if (currentBattle == null) {
			currentBattle = (BattleMode) Resources.Load ("Scriptables/Battle/Storages/BattleMode", typeof(BattleMode));
		}
	}

	public static void InitializeCurrentData(){
		LoadCurrentBattle ();
		currentBattle.InitializeFromMasterStateData ();
	}

	// "Was this the last goal of the match?"
	public static void AddGoal(GoalData goalData){
		LoadCurrentBattle ();
		Debug.Log ("Goal detected to goal ID: " + goalData.goalPlayerID + " by player ID " + goalData.giverPlayerID);
		currentBattle.DoGoal (goalData);
	}

	// "Did game end?"
	public static void AdvanceTime(float deltaTime){
		LoadCurrentBattle ();
		currentBattle.AdvanceTime (deltaTime);
	}
}
