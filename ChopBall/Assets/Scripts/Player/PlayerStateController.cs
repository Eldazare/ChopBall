using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerStateController {

	static PlayerStateData[] states;

	public static void GetStates(){
		states = Resources.LoadAll("Scriptables/Players/StateData", typeof(PlayerStateData)).Cast<PlayerStateData>().ToArray();
		/*
		PlayerStateData[] test = loads.ToArray();
		Debug.Log (test);
		foreach (var nb in loads) {
			states.Add (nb);
		}
		*/
	}

	public static void ChooseCharacter(int playerID, int characterID){
		if (states == null) {
			GetStates ();
		}
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
