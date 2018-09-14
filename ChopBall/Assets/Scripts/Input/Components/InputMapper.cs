using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputMapper : MonoBehaviour {

	// Attach this to a panel which you want to control the remapping

	public bool enableMegaTranslators = false; // for listening for remaps
	public int numberOfAxes = 6;
	public int numberOfPlayers = 2;
	public GameObject remapButtonPrefab;
	private List<List<KeyCode>> keyCodesList;
	private List<List<String>> axisStringList;
	private List<ButtonCommand> listenedCommands;

	private float axisAmount;
	// FLOW: ? 
	// Generate buttons
	// Wait for button message
	// Prime yourself for an input (Custom translator which has all the buttons) 
	// (Maybe disable translator for the time being)
	// Run custom translator until input



	public void GenerateRemapButtons(){
		foreach (ButtonCommand command in Enum.GetValues(typeof(ButtonCommand))) {
			GameObject commandButton = Instantiate (remapButtonPrefab, gameObject.transform) as GameObject;
			commandButton.GetComponent<RemapCursorButton> ().Initialize (command, ReceiveButtonPress);
		}
	}



	public void ReceiveButtonPress(ButtonCommand command, int playerID){
		listenedCommands [playerID - 1] = command;
	}

	void Awake(){
		InitializeAxesAndButtons ();
		if (remapButtonPrefab != null) {
			GenerateRemapButtons ();
		}
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
			if(axisAmount > 0.5 || axisAmount < -0.5){
				Debug.Log (axisStringList[playerIndex][i] + " is "+ axisAmount);
			}
		}
	}

	private void InitializeAxesAndButtons(){
		axisStringList = new List<List<string>> ();
		keyCodesList = new List<List<KeyCode>> ();
		listenedCommands = new List<ButtonCommand> ();
		for (int i = 0; i < numberOfPlayers; i++) {
			List<string> newList = new List<string> ();
			for (int j = 0; j < numberOfAxes; j++) {
				newList.Add ("P" + (i+1) + "Axis" + (j+1));
			}
			axisStringList.Add (newList);
			keyCodesList.Add (InputButtonsReference.GetButtonsForPlayer (i+1));
			listenedCommands.Add (ButtonCommand.None);
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
				if (listenedCommands [playerIndex] != ButtonCommand.None) {
					InputStorageController.SetAButtonToStorage (playerIndex + 1, listenedCommands [playerIndex], keyCode);
					Debug.Log("Player "+(playerIndex+1)+" registered button "+keyCode+" for command "+listenedCommands[playerIndex]+".");
					listenedCommands[playerIndex] = ButtonCommand.None;
				}
			}
		}
	}
}
