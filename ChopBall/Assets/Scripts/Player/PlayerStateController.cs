using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerStateController {

	static PlayerStateData[] states;

	private static void LoadStates(){
		if (states == null) {
			states = Resources.LoadAll ("Scriptables/Players/StateData", typeof(PlayerStateData)).Cast<PlayerStateData> ().ToArray ();
		}

	}

	public static PlayerStateData[] GetAllStates(){
		LoadStates ();
		return states;
	}

	public static PlayerStateData GetAState(int playerID){
		LoadStates ();
		return states [playerID - 1];
	}

	public static void ChooseCharacter(int playerID, int characterID){
		LoadStates ();
		PlayerStateData data = states [playerID-1];
		if (data.characterChoice != characterID) {
			data.characterChoice = characterID;
			data.XYmovementLocked = true;

		} else {
			data.characterChoice = -1;
			data.XYmovementLocked = false;
		}
		Debug.Log ("Set");
	}
}
