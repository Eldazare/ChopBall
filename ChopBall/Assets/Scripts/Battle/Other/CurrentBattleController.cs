using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentBattleController  {


	// TODO: Add events and a monobehaviour to control events (and listen to goals etc.)

	private static CurrentBattleStorage currentBattle;

	private static void LoadCurrentBattle(){
		if (currentBattle == null) {
			currentBattle = (CurrentBattleStorage) Resources.Load ("Scriptables/Battle/Storage/CurrentBattleStorage", typeof(CurrentBattleStorage));
		}
	}

	// "Was this the last goal of the match?"
	public static bool AddGoal(GoalData goalData){
		LoadCurrentBattle ();
		// TODO: Parse from currentBattle for how to insert
		return false;
	}

	// "Did game end?"
	public static bool AdvanceTime(float deltaTime){
		LoadCurrentBattle ();
		if (currentBattle.useTimer) {
			currentBattle.secondsLeft -= deltaTime;
			if (currentBattle.secondsLeft < 0) {
				currentBattle.minutesLeft -= 1;
				if (currentBattle.minutesLeft < 0) {
					return true;
				}
			}
		}
		return false;
	}
}


public class GoalData{
	public int giverPlayerID;
	public int goalPlayerID;
	public int teamGoalID;
}
