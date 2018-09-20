using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentBattleController  {


	// TODO: Add events and a monobehaviour to control events (and listen to goals etc.)

	private static CurrentBattleStorage currentBattle;

	private static void LoadCurrentBattle(){
		if (currentBattle == null) {
			currentBattle = (CurrentBattleStorage) Resources.Load ("Scriptables/Battle/Storages/CurrentBattleStorage", typeof(CurrentBattleStorage));
		}
	}

	public static void InitializeCurrentData(){
		LoadCurrentBattle ();

	}

	// "Was this the last goal of the match?"
	public static int AddGoal(GoalData goalData){
		LoadCurrentBattle ();
		currentBattle.roundsLeft -= 1;
		return currentBattle.roundsLeft;
	}

	// "Did game end?"
	public static int AdvanceTime(float deltaTime){
		LoadCurrentBattle ();
		if (currentBattle.useTimer) {
			currentBattle.secondsLeft -= deltaTime;
			if (currentBattle.secondsLeft < 0) {
				currentBattle.minutesLeft -= 1;
				if (currentBattle.minutesLeft < 0) {
					currentBattle.roundsLeft -= 1;
				}
			}
		}
		return currentBattle.roundsLeft;
	}
}
