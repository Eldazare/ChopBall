using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoiceButtonGenerator : MonoBehaviour {

	public GameObject stageButtonPrefab;
	public GameObject stageButtonPanel; // Should contain N-amount of buttons. Optional.

	/*
	// TODO: Comment/Remove, FOR DEBUGGING ONLY
	void Start(){
		InitializeButtonsFromPanel (StageTag.T1v1);
	}
	*/

	public void GenerateButtons(StageTag primaryTag){
		StageData[] stages = StageDataController.GetStages ();
		foreach (StageData stage in stages) {
			StageChoiceButton button = Instantiate (stageButtonPrefab, gameObject.transform).GetComponent<StageChoiceButton>();
			button.Initialize (stage);
			button.CheckPrimaryTag (primaryTag);
		}
	}

	public void InitializeButtonsFromPanel(StageTag primaryTag){
		StageData[] stages = StageDataController.GetStages ();
		StageChoiceButton[] buttons = stageButtonPanel.GetComponentsInChildren<StageChoiceButton> ();
		int indexCap;
		if (stages.Length > buttons.Length) {
			indexCap = buttons.Length;
		} else {
			indexCap = stages.Length;
		}
		Debug.Log ("Stages found: " + stages.Length + " | Buttons found: " + buttons.Length);
		for (int i = 0; i < indexCap; i++) {
			buttons [i].Initialize (stages [i]);
			buttons [i].CheckPrimaryTag (primaryTag);
		}
		for (int i = indexCap; i<buttons.Length;i++){
			buttons [i].gameObject.SetActive (false);
		}
	}
}
