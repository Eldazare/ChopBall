using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageLoading : MonoBehaviour {

	// testing relevance between StageData and scene transition + battle scene initialization
	void Start () {
		MasterStateController.GetTheMasterData ().SetDefaults ();
		MasterStateController.GetTheMasterData ().GoToBattle ();
	}

}
