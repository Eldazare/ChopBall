using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterStateController {

	private static MasterStateData masterData;

	private static void LoadMasterData(){
		if (masterData == null) {
			masterData = Resources.Load ("Scriptables/Player/StateData/MasterStateData", typeof(MasterStateData)) as MasterStateData;
		}
	}

	public static MasterStateData GetTheMasterData(){
		LoadMasterData ();
		return masterData;
	}

	public static void WriteStageName(string mapName){
		LoadMasterData ();
		masterData.stageNameFinal = mapName;
	}
}
