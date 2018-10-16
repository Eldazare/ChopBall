using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStateData : ScriptableObject {

	private GameEvent OnCharacterChosen;

	public bool active = false;
	public bool CharacterLocked = false;
	public bool characterChoosing = false;
	public int characterChoice = -1;
	public int team = -1; // From 0 to 3
	public string stageNameChoice = "";
	//TODO, more stuff


	public void SetDefaultValues(){
		OnCharacterChosen = ((PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData))).OnCharacterChosen;
		active = false;
		CharacterLocked = false;
		characterChoosing = false;
		characterChoice = -1;
		team = -1;
		stageNameChoice = "";
		// TODO add as they come
	}

	public void ChooseCharacter(int charID){
		if (characterChoice != 1 && characterChoice != charID) {
			characterChoice = charID;
			CharacterLocked = true;
			active = true;
		} else {
			characterChoice = -1;
			CharacterLocked = false;
			active = false;
		}
		OnCharacterChosen.Raise ();
	}
}
