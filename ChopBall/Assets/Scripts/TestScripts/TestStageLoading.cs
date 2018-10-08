using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageLoading : MonoBehaviour {

	// testing relevance between StageData and scene transition + battle scene initialization
	void Start () {
		PlayerStateController.SetDefaultsAll ();
		MasterStateController.GetTheMasterData ().SetDefaults ();
	}

	public void GetInput(InputModel model){
		if (model.Submit) {
			PlayerStateController.SetStateActive (model.playerID - 1);
			Debug.Log ("State set active: " + model.playerID);
		}
		if (model.Start) {
			MasterStateController.GetTheMasterData ().GoToBattle ();
		}
	}



}
