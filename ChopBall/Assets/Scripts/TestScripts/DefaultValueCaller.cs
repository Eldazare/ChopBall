using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DefaultValueCaller {

	public static void RunAll(){
		DefaultButtonsAll ();
		DefaultPlayerStatesAll ();
	}

	public static void DefaultButtonsAll(){
		InputStorage[] storages = Resources.LoadAll ("Scriptables/Input/ButtonStorage").Cast<InputStorage> ().ToArray ();
		if (storages == null) {
			Debug.LogWarning ("Input storages not found");
		} else {
			foreach (InputStorage stor in storages) {
				stor.SetDefaultButtons ();
			}
		}
	}

	public static void DefaultPlayerStatesAll(){
		PlayerStateData[] states = Resources.LoadAll ("Scriptables/Players/StateData").Cast<PlayerStateData> ().ToArray ();
		if (states == null) {
			Debug.LogWarning ("PlayerStateDatas not found for defaultValueCaller");
		} else {
			foreach (PlayerStateData state in states) {
				state.SetDefaultValues ();
			}
		}
	}


}
