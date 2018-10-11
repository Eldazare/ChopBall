using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestStageLoading : MonoBehaviour {


	public List<Image> imageList;
	public PlayerBaseData playerBaseData;
	// testing relevance between StageData and scene transition + battle scene initialization
	void Start () {
		PlayerStateController.SetDefaultsAll ();
		MasterStateController.GetTheMasterData ().SetDefaults ();
	}

	public void GetInput(InputModel model){
		if (model.Submit) {
			PlayerStateController.SetStateActive (model.playerID - 1);
			imageList [model.playerID - 1].color = playerBaseData.playerColors [model.playerID - 1];
			imageList[model.playerID-1].gameObject.SetActive (true);
			Debug.Log ("State set active: " + model.playerID);
		}
		if (model.Start) {
			MasterStateController.GetTheMasterData ().GoToBattle ();
		}
	}



}
