using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterStateController {

	// STAGE SELECTION:
	// Before going into stage selection screen, UI reads MASTERDATA and determines which screen to use (pre StageChooseType)
	// Each screen should have conditions for calling ChooseStage(), and if that returns true, then change Scene.
	// Scene change and BattleLoader TBA.

	private static MasterStateData masterData;

	private static void LoadMasterData(){
		if (masterData == null) {
			masterData = Resources.Load ("Scriptables/Players/StateData/MasterStateData", typeof(MasterStateData)) as MasterStateData;
			Random.InitState((int)System.DateTime.Now.Ticks);
		}
		if (masterData == null) {
			Debug.LogError ("MasterData is still null");
		}
	}

	public static MasterStateData GetTheMasterData(){
		LoadMasterData ();
		return masterData;
	}

	public static void SetRandomStagePreset(List<string> preset){
		LoadMasterData ();
		masterData.randomStagePreset = preset;
	}

	public static void WriteStageName(string mapName){
		LoadMasterData ();
		masterData.stageNameFinal = mapName;
	}

	public static bool ChooseStage(){
		LoadMasterData ();
		switch (masterData.stageChoiceType) {
		case StageChoiceType.individualRandom:
			return RandomizeStageSelectFromPlayers ();
		case StageChoiceType.masterSingle:
			if (masterData.stageNameFinal != "") {
				return true;
			} else {
				Debug.LogError ("Masterdata doesn't have stage selected");
				return false;
			}
		case StageChoiceType.randomPreset:
			return RandomizeStageSelectFromPreset ();
		default:
			Debug.LogError ("Undefined StageChoiceType used: " + masterData.stageChoiceType);
			return false;
		}
	}

	private static bool RandomizeStageSelectFromPlayers(){
		//LoadMasterData ();
		List<string> randomList = new List<string> ();
		foreach(PlayerStateData pData in PlayerStateController.GetAllStates()){
			if (pData.stageNameChoice != "") {
				randomList.Add (pData.stageNameChoice);
			}
		}
		if (randomList.Count > 0) {
			int chosenIndex = Random.Range (0, randomList.Count);
			masterData.stageNameFinal = randomList [chosenIndex];
			return true;
		} else {
			Debug.LogError ("No player has chosen any stages");
			return false;
		}
	}

	private static bool RandomizeStageSelectFromPreset(){
		//LoadMasterData ();
		List<string> preset = masterData.randomStagePreset;
		if (preset != null) {
			int chosenIndex = Random.Range (0, preset.Count);
			masterData.stageNameFinal = preset [chosenIndex];
			return true;
		} else {
			Debug.LogError ("RandomStagePreset is null.");
			return false;
		}
	}

	public static void GoToBattle(){
		LoadMasterData ();
		if (ChooseStage()) {
			masterData.GoToBattle ();
		}
	}
}
