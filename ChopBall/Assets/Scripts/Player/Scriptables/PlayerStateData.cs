using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStateData : ScriptableObject {

	private bool loaded = false;
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
		active = false;
		CharacterLocked = false;
		characterChoosing = false;
		characterChoice = -1;
		team = -1;
		stageNameChoice = "";
		// TODO add as they come
	}

	private void GetBaseDataInfo(){
		if (!loaded) {
			playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
			OnCharacterChosen = playerBaseData.OnCharacterChosen;
			OnTeamChanged = playerBaseData.OnTeamChanged;
			loaded = true;
		}
	}

	public void ChooseCharacter(int charID){
		GetBaseDataInfo ();
		if (active) {
			if (charID != -1 && characterChoice != charID) {
				characterChoice = charID;
				CharacterLocked = true;
			} else {
				characterChoice = -1;
				CharacterLocked = false;
			}
			OnCharacterChosen.Raise ();
		}
	}

	public void ChangeTeam(bool incDec){
		GetBaseDataInfo ();
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
