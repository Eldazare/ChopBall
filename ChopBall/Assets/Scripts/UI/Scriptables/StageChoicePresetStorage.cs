using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageChoicePresetStorage : ScriptableObject {

	public List<int> preset;

	public void DefaultAllStages(){
		preset = new List<int> ();
		for (int i = 0; i < StageDataController.GetStages ().Length; i++) {
			preset.Add (i);
		}
	}
}
