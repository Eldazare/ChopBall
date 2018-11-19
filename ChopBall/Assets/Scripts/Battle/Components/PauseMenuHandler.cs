using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {

	public GameEvent OnPauseGame;
	public GameEvent OnGameEnd;
	private int pausedPlayerID = -1;
	private bool paused = false;
	private bool pausePressed = false;

	public void GetInput(InputModel model){
		if (!pausePressed) {
			if (paused) {
				if (model.playerID == pausedPlayerID){
					if (model.Start) {
						PauseTheGame ();
						paused = false;
						pausePressed = true;
					}
					if (model.Select) {
						OnGameEnd.Raise ();
					}
				}
			} else {
				if (model.Start) {
					pausedPlayerID = model.playerID;
					PauseTheGame ();
					paused = true;
					pausePressed = true;
				}
			}
		}
		if (model.playerID == pausedPlayerID && pausePressed != model.Start) {
			pausePressed = false;
			if (!paused) {
				pausedPlayerID = -1;
			}
			Debug.Log (pausePressed);
		}

	}

	private void PauseTheGame(){
		OnPauseGame.Raise ();
	}
}
