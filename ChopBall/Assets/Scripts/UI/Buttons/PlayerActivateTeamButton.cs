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


	private bool latePRight;
	private bool latePLeft;
	private bool lateStart;

	override
	protected void Awake(){
		base.Awake ();
		text = GetComponentInChildren<Text> ();
		stateData = PlayerStateController.GetAState (playerID);
		baseStr = "Player: " + playerID + "\n";
	}

	void OnEnable(){
		bool latePRight = true;
		bool latePLeft = true;
		bool lateStart = true;
		SetTeamPart ();
		SetCharChoiceStr ();
		stateData.CheckTeamConstraints ();
	}

	public void GetInput(InputModel model){
		if (IsButtonTrue(model.PaddleRight, latePRight,out latePRight)) {
			ChangeTeam (true);
			SetTeamPart ();
		}
		if (IsButtonTrue(model.PaddleLeft, latePLeft,out latePLeft)) {
			ChangeTeam (false);
			SetTeamPart ();
		} 
		if (IsButtonTrue(model.Start, lateStart,out lateStart)) {
			if (stateData.active) {
				ActivateState(false);
			} else {
				ActivateState (true);
			}
		}
	}

	private bool IsButtonTrue(bool button, bool late, out bool lateOut){
		if (button && !late) {
			lateOut = true;
			return true;
		} else if (!button && late) {
			lateOut = false;
		} else {
			lateOut = late;
		}
		return false;
	}

	private void ActivateState(bool active){
		PlayerStateController.SetStateActive (playerID - 1, active);
		GetComponent<Image>().enabled = active;
		text.enabled = active;
	}

	private void ChangeTeam(bool incDec){
		stateData.ChangeTeam (incDec);
	}

	public void SetTeamPart(){
		if (stateData.team != -1) {
			teamPart = "Team: " + stateData.team + "\n";
		} else {
			teamPart = "\n";
		}
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
