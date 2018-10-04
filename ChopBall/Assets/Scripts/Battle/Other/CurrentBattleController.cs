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
		
	public static void AddGoal(GoalData goalData){
		LoadCurrentBattle ();
		Debug.Log ("Goal detected to goal ID: " + goalData.goalPlayerID + " by player ID " + goalData.giverPlayerIDs[0]);
		currentBattle.DoGoal (goalData);
	}
		
	public static void AdvanceTime(float deltaTime){
		LoadCurrentBattle ();
		currentBattle.AdvanceTime (deltaTime);
	}

	public static ATime GetATime(){
		LoadCurrentBattle ();
		return new ATime (currentBattle.minutesLeft, currentBattle.secondsLeft);
	}

	public static List<CompetitorContainer> GetCompetitors(){
		LoadCurrentBattle ();
		return currentBattle.competitors;
	}
}
