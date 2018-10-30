using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class CharacterStateController {

	private static CharacterState[] states;

	private static void LoadStates(){
		if (states == null) {
			states = Resources.LoadAll ("Scriptables/States", typeof(CharacterState)).Cast<CharacterState> ().ToArray ();
			string[] names = Enum.GetNames (typeof(CharacterStateEnum));
			if (states.Length != names.Length) {
				Debug.LogError ("Character state length mismatch between enum and scriptables");
			}
			for (int i = 0; i<states.Length;i++) {
				if (states [i].identifier.ToString () != names [i]) {
					Debug.LogError ("Character State identifier misalign at state: " + states [i].name);
				}
			}
		}
	}

	public static CharacterState[] GetCharStates(){
		LoadStates ();
		return states;
	}
}
