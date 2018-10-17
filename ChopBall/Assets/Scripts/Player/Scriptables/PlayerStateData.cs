using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStateData : ScriptableObject {

	private PlayerBaseData playerBaseData;
	private GameEvent OnCharacterChosen;
	private GameEvent OnTeamChanged;

	public bool active = false;
	public bool CharacterLocked = false;
	public bool characterChoosing = false;
	public int characterChoice = -1;
	public int team = -1; // From 0 to 3
	public string stageNameChoice = "";
	//TODO, more stuff


	public void SetDefaultValues(){
		playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
		OnCharacterChosen = playerBaseData.OnCharacterChosen;
		OnTeamChanged = playerBaseData.OnTeamChanged;
		active = false;
		CharacterLocked = false;
		characterChoosing = false;
		characterChoice = -1;
		team = -1;
		stageNameChoice = "";
		// TODO add as they come
	}

	public void ChooseCharacter(int charID){
		if (active) {
			if (characterChoice != 1 && characterChoice != charID) {
				characterChoice = charID;
				CharacterLocked = true;
				//active = true;
			} else {
				characterChoice = -1;
				CharacterLocked = false;
				//active = false;
			}
			OnCharacterChosen.Raise ();
		}
	}

	public void ChangeTeam(bool incDec){
		if (active) {
			if (incDec) {
				team++;
				if (team >= MasterStateController.GetMaxNumberOfTeams ()) {
					team = 0;
				}
			} else {
				team--;
				if (team < 0) {
					team = MasterStateController.GetMaxNumberOfTeams () - 1;
				}
			}
			OnTeamChanged.Raise ();
		}
	}

	public void CheckTeamConstraints(){
		if (team >= MasterStateController.GetMaxNumberOfTeams ()) {
			team = MasterStateController.GetMaxNumberOfTeams () - 1;
		}
	}
}
