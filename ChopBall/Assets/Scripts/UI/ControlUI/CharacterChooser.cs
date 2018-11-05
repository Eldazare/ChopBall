using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChooser : MonoBehaviour {

	public int playerID;
	private PlayerStateData playerStateData;
	private int currentChoice;


	void OnEnable(){
		playerStateData = PlayerStateController.GetAState (playerID);
		// SET INPUT BOOLS TO TRUE
	}

	public void GetInput(InputModel model){
		// TODO:
	}

	public void IncDecCurrentChoice(bool incDec){
		
	}

	private void ConfirmChoice(){
		PlayerStateController.ChooseCharacter (playerID, currentChoice);
	}
}
