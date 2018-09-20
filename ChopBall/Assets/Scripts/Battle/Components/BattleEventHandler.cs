using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventHandler : MonoBehaviour {

	public GameEvent GameEndEvent;
	private bool paused = false;
	private bool startGame = false;

	public void PauseGame(){
		if (paused == true) {
			paused = false;
		} else {
			paused = true;
		}
	}

	public void StartGame(){
		startGame = true;
	}

	public void PassGoalData(GoalData goalData){
		if (CurrentBattleController.AddGoal (goalData) == 0) {
			GameEndEvent.Raise ();
		}
	}

	void Update(){
		if (!startGame) {
			if (!paused) {
				if (CurrentBattleController.AdvanceTime (Time.deltaTime) == 0) {
					GameEndEvent.Raise ();
				}
			}
		}
	}
}
