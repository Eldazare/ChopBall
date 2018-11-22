using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChooser : MonoBehaviour {

	StageData[] stages;
	StageTag currentTag;

	void OnEnable(){
		StageData[] stages = StageDataController.GetStages ();
		StageTag currentTag = StageTagHandler.GetTagsFromCurrentPlayers ();
	}
}
