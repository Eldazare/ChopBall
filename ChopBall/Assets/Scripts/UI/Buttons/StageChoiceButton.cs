using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoiceButton : _CursorButton {

	private string stageName;

	public void Initialize(string mapName){
		this.stageName = mapName;
	}

	override
	public void Click(int playerID){
		if (playerID > 0) {
			PlayerStateController.ChooseStage (playerID, stageName);
		} else if (playerID == 0) {
			MasterStateController.WriteStageName (stageName);
		} else {
			Debug.LogError("Invalid playerID given :" + playerID);
		}
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hovering over stage choice: " + stageName);
	}
}
