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
			data.CharacterLocked = true;
		} else {
			data.characterChoice = -1;
			data.CharacterLocked = false;
		}
	}

	public static void ChooseStage(int playerID, string stageName){
		LoadStates ();
		PlayerStateData data = states [playerID - 1];
		data.stageNameChoice = stageName;
	}

	public static void SetDefaultsAll(){
		LoadStates ();
		foreach (PlayerStateData state in states) {
			state.SetDefaultValues ();
		}
	}

	public static void SetDefaults(int playerID){
		LoadStates ();
		states [playerID - 1].SetDefaultValues ();
	}

	public static void SetStateActive(int stateIndex){
		LoadStates ();
		states [stateIndex].active = true;
	}

	public static int GetNumberOfTeams(){
		LoadStates ();
		List<int> teamList = new List<int> ();
		foreach (var state in states) {
			if (state.active) {
				if (!teamList.Contains (state.team)) {
					teamList.Add (state.team);
				}
			}
		}
		return teamList.Count;
	}

	public static int GetNumberOfActivePlayers(){
		LoadStates ();
		int activePlayers = 0;
		foreach (var state in states) {
			if (state.active) {
				if (state.characterChoice > -1) {
					activePlayers++;
				}
			}
		}
		return activePlayers;
	}

	public static void SetCharacterChoosing(bool bo){
		LoadStates ();
		foreach (var state in states) {
			state.characterChoosing = bo;
		}
	}
}
