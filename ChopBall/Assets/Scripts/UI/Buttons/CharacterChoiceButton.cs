using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoiceButton : _CursorButton {

	public int characterID;

	override
	public void Click(int playerID){
		if (playerID > 0) {
			if (characterID > 0) { // playerCursor
				PlayerStateController.ChooseCharacter (playerID, characterID);
			} else {
				Debug.LogError ("No characterID");
			}
		} else if (playerID == 0) {
			Debug.Log ("MasterCursor can't choose character");
		} else {
			Debug.LogError("Invalid playerID given :" + playerID);
		}
	}

	override
	public void OnHoverEnter(int playerID){
		base.OnHoverEnter (playerID);
		Debug.Log ("Player " + playerID + " just entered characterChoice with ID " + characterID);
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hover over choice ID: " + characterID);
	}

	override
	public void OnHoverExit(int playerID){
		Debug.Log ("Player " + playerID + " just exited characterChoice with ID " + characterID);
	}
}
