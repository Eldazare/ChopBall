using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoicePresetHandler : MonoBehaviour {

	public List<ToggleStageButton> buttons;

	public StageTag currentTag;
	private StageChoicePresetStorage currentPreset;
	private List<StageData> stages;

	public void ToggleStage(int index){
		if (currentPreset.preset.Contains (index)) {
			currentPreset.preset.Remove (index);
			buttons [index].Toggled (true);
		} else {
			currentPreset.preset.Add (index);
			buttons [index].Toggled (false);
		}
	}

	void Awake(){
		
		stages = new List<StageData>(StageDataController.GetStages ());
	}

	public void ChoosePresetForEdit(int storageIndex){
		// TODO: Get storage from manager and initialize also the StageTagType choice
		InitializeButtonsFromPanel(currentPreset.tagForPreset);
	}


	public void SavePreset(int storageIndex){
		// TODO:
	}


	public void InitializeButtonsFromPanel(StageTag primaryTag){
		int indexCap;
		if (stages.Count> buttons.Count) {
			indexCap = buttons.Count;
		} else {
			indexCap = stages.Count;
		}
		Debug.Log ("Stages found: " + stages.Count + " | Buttons found: " + buttons.Count);
		Debug.Log ("Index cap: " + indexCap);
		for (int i = 0; i < indexCap; i++) {
			buttons [i].Initialize (stages[i]);
			buttons [i].CheckPrimaryTag (primaryTag);
			buttons [i].SetDelegate (ToggleStage, i);
		}
		for (int i = indexCap; i<buttons.Count;i++){
			buttons [i].gameObject.SetActive (false);
		}
	}
}
