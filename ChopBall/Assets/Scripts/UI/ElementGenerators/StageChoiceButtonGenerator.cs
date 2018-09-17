using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoiceButtonGenerator : MonoBehaviour {

	public GameObject stageChoiceButtonPrefab;
	public GameObject stageChoiceButtonPanel; // Should contain N-amount of buttons. Optional.

	public void GenerateButtons(StageTag primaryTag){
		StageData[] stages = StageDataController.GetStages ();
		foreach (StageData stage in stages) {
			StageChoiceButton button = Instantiate (stageChoiceButtonPrefab, gameObject.transform).GetComponent<StageChoiceButton>();
			button.Initialize (stage);
			button.CheckPrimaryTag (primaryTag);
		}
	}

	public void InitializeButtonsFromPanel(StageTag primaryTag){
		StageData[] stages = StageDataController.GetStages ();
		StageChoiceButton[] buttons = stageChoiceButtonPanel.GetComponentsInChildren<StageChoiceButton> ();
		int indexCap;
		if (stages.Length > buttons.Length) {
			indexCap = stages.Length;
		} else {
			indexCap = buttons.Length;
		}
		Debug.Log ("Stages found: " + stages.Length + " | Buttons found: " + buttons.Length);
		for (int i = 0; i < indexCap; i++) {
			buttons [i].Initialize (stages [i]);
			buttons [i].CheckPrimaryTag (primaryTag);
		}
	}
}
