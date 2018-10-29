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
		if (goalData.giverPlayerIDs.Count > 0) {
			Debug.Log ("Goal detected to goal ID: " + goalData.goalPlayerID + " by player ID " + goalData.giverPlayerIDs [0]);
		} else {
			Debug.Log("Goal detected to goal ID: " + goalData.goalPlayerID + " and no goal giver");
		}
		currentBattle.DoGoal (goalData);
	}
		
	public static void ProgressTime(float deltaTime){
		LoadCurrentBattle ();
		currentBattle.ProgressTime (deltaTime);
	}

	public static ATime GetATime(){
		LoadCurrentBattle ();
		ATime time = new ATime (currentBattle.minutesLeft, currentBattle.secondsLeft);
		if (currentBattle.suddenDeath) {
			time.str = "Sudden Death!";
		}
		return time;
	}

	public static List<CompetitorContainer> GetCompetitors(){
		LoadCurrentBattle ();
		return currentBattle.competitors;
	}

	public static List<TeamContainer> GetTeams(){
		LoadCurrentBattle ();
		return currentBattle.teams;
	}
}
