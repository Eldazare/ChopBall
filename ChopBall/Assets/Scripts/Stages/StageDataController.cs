using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class StageDataController {

	private static StageData[] stages;

	private static void LoadStages(){
		if (stages == null) {
			stages = Resources.LoadAll ("Scriptables/Stages").Cast<StageData> ().ToArray ();
		}
	}

	public static StageData[] GetStages(){
		LoadStages ();
		return stages;
	}
}
