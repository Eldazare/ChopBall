﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActivateTeamButton : MonoBehaviour {

	// TODO?: Press start to press proceed button?


	public int playerID;

	private PlayerBaseData playerBaseData;
	private PlayerStateData stateData;
	private Text text;
	private Image image;
	private string baseStr;
	private string teamPart;
	private string characterChoicePart;


	private bool latePRight;
	private bool latePLeft;
	private bool lateSelect;

	protected void Awake(){
		//base.Awake ();
		image = GetComponent<Image> ();
		text = GetComponentInChildren<Text> ();
		stateData = PlayerStateController.GetAState (playerID);
		playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData");
		baseStr = "Player: " + playerID + "\n";
	}

	void OnEnable(){
		bool latePRight = true;
		bool latePLeft = true;
		bool lateStart = true;
		SetTeamPart ();
		SetCharChoicePart ();
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
		if (IsButtonTrue(model.Select, lateSelect,out lateSelect)) {
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
			SetColor (playerBaseData.teamColors [stateData.team]);
		} else {
			teamPart = "\n";
			SetColor (playerBaseData.playerColors [playerID-1]);
		}
		SetString ();
	}

	public void SetCharChoicePart(){
		if (stateData.characterChoice == -1) {
			characterChoicePart = "No Character";
		} else {
			characterChoicePart = "Character: "+stateData.characterChoice;
		}
		characterChoicePart += "\n";
		SetString ();
	}

	private void SetString(){
		text.text = baseStr + teamPart + characterChoicePart;
	}

	public void SetColor(Color32 color){
		this.image.color = color;
	}
}
