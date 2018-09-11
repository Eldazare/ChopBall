using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoiceButton : _CursorButton {

	public int characterID;

	override
	public void Click(int playerID){
		if (characterID != null) {
			PlayerStateController.ChooseCharacter (playerID, characterID);
		} else {
			Debug.LogError ("No characterID");
		}
	}
}
