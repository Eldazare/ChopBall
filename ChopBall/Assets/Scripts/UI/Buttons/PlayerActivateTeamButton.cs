using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActivateTeamButton : _CursorButton {

	public int playerID;

	private PlayerStateData stateData;
	private Text text;
	private string baseStr;
	private string teamPart;
	private string characterChoicePart;

	void Start(){
		text = GetComponentInChildren<Text> ();
		stateData = PlayerStateController.GetAState (playerID);
		baseStr = "Player: " + playerID + "\n";
		teamPart = "\n";
		SetCharChoiceStr ();
	}

	// TODO: Check continous button input
	public void GetInput(InputModel model){
		if (model.PaddleRight) {
			ChangeTeam (true);
			SetTeamPart ();
		}
		if (model.PaddleLeft) {
			ChangeTeam (false);
			SetTeamPart ();
		} if (model.Start) {
			PlayerStateController.SetStateActive (playerID - 1);
			GetComponent<Image>().enabled = true;
			text.enabled = true;
		}
	}

	private void ChangeTeam(bool incDec){
		// TODO:
	}

	public void SetTeamPart(){
		teamPart = "Team: " + stateData.team + "\n";
		SetString ();
	}

	public void SetCharChoiceStr(){
		if (stateData.characterChoice == -1) {
			characterChoicePart = "Character not chosen.";
		} else {
			characterChoicePart = "Char choice: "+stateData.characterChoice;
		}
		characterChoicePart += "\n";
		SetString ();
	}

	private void SetString(){
		text.text = baseStr + teamPart + characterChoicePart;
	}
}
