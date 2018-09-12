using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class InputButtonsReference {

	private static int buttonCount = 20;
	private static List<List<KeyCode>> theReference;
	private static bool initialized = false;


	private static void Initialize(){
		if (!initialized) {
			theReference = new List<List<KeyCode>> ();
			int storageCount = InputStorageController.GetAllStorages ().Length;
			for (int i = 0; i < storageCount; i++) {
				theReference.Add (ReadAllButtonsForPlayer (i + 1));
			}
			initialized = true;
		}
	}

	private static List<KeyCode> ReadAllButtonsForPlayer(int playerID){
		List<KeyCode> returnee = new List<KeyCode> ();
		for (int i = 0; i < buttonCount; i++) {
			returnee.Add ((KeyCode)Enum.Parse (typeof(KeyCode), "Joystick" + playerID + "Button" + i));
		}
		return returnee;
	}

	public static List<KeyCode> GetButtonsForPlayer(int playerID){
		Initialize ();
		return theReference [playerID];
	}
}
