using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputMapper : MonoBehaviour {

	public bool enableMegaTranslators = false;
	public int numberOfAxes = 6;
	public int numberOfPlayers = 2;
	private List<List<KeyCode>> keyCodesList;
	private List<List<String>> axisStringList;

	private float axisAmount;
	// FLOW: ? 
	// Generate buttons
	// Wait for button message
	// Prime yourself for an input (Custom translator which has all the buttons) 
	// (Maybe disable translator for the time being)
	// Run custom translator until input



	public void GenerateChangeObjects(int playerID){
		foreach (ButtonCommand command in Enum.GetValues(typeof(ButtonCommand))) {
			
		}
	}



	public void ReceiveButtonPress(ButtonCommand command, KeyCode newButton){
		
	}

	void Awake(){
		InitializeAxesAndButtons ();
	}

	void Update(){
		if (enableMegaTranslators) {
			MegaTranslatorAxes ();
			MegaTranslatorButtons ();
		}
	}

	private void MegaTranslatorAxes(){
		for (int i = 0; i < numberOfPlayers; i++) {
			SuperTranslatorAxes (i);
		}
	}

	private void SuperTranslatorAxes(int playerIndex){
		for (int i = 0; i < numberOfAxes; i++) {
			axisAmount = Input.GetAxisRaw (axisStringList[playerIndex][i]);
			if(axisAmount != 0){
				Debug.Log (axisStringList[playerIndex][i] + " is "+ axisAmount);
			}
		}
	}

	private void InitializeAxesAndButtons(){
		axisStringList = new List<List<string>> ();
		keyCodesList = new List<List<KeyCode>> ();
		for (int i = 0; i < numberOfPlayers; i++) {
			List<string> newList = new List<string> ();
			for (int j = 0; j < numberOfAxes; j++) {
				newList.Add ("P" + (i+1) + "Axis" + (j+1));
			}
			axisStringList.Add (newList);
			keyCodesList.Add (InputButtonsReference.GetButtonsForPlayer (i+1));
		}
	}

	private void MegaTranslatorButtons(){
		for (int i = 0; i < numberOfPlayers; i++) {
			SuperTranslatorButtons (i);
		}
	}

	private void SuperTranslatorButtons(int playerIndex){
		foreach (KeyCode keyCode in keyCodesList[playerIndex]) {
			if (Input.GetKey (keyCode)) {
				Debug.Log ("Key " + keyCode.ToString () + " registered");
			}
		}
	}
}
