using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class StagePresetController  {

	private static StageChoicePresetStorage[] presets;

	private static void LoadStagePresets(){
		if (presets == null) {
			presets = Resources.LoadAll ("Resources/Stages/Presets", typeof(StageChoicePresetStorage)).Cast<StageChoicePresetStorage> ().ToArray ();
		}
	}
}
