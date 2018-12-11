using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEventHandler : MonoBehaviour {

	private bool paused = false;
	private bool startGame = false;
	float time;

	public void PauseGame(){
		if (paused == true) {
			paused = false;
		} else {
			paused = true;
		}
	}

	public void StartGame(){
		Debug.Log ("STARTED");
		startGame = true;
	}

	public void PassGoalData(GoalData goalData){
		CurrentBattleController.AddGoal (goalData);
	}

	void Update(){
		if (startGame) {
			if (!paused) {
				time = Time.deltaTime;
				BuffController.ProgressTime (time);
				CurrentBattleController.ProgressTime (time);
				RuntimeModifierController.ProgressTime (time);
			}
		}
	}

	public void EndMatch(){
		BuffController.EndAllBuffs ();
		SceneManager.LoadScene ("MatchEndScene");
	}
}
