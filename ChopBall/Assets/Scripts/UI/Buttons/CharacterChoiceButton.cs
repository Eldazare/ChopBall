using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoiceButton : _CursorButton {

	public int characterID;

	override
	public void Click(int playerID){
		if (characterID > 0) {
			PlayerStateController.ChooseCharacter (playerID, characterID);
		} else {
			Debug.LogError ("No characterID");
		}
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hover over choice ID: " + characterID);
	}
}
