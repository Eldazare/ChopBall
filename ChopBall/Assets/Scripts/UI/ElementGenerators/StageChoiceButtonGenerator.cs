using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChoiceButtonGenerator : MonoBehaviour {

	// EDITOR BUG: Editor start: OnEnable triggers earlier than button Awake.
	// Interestingly, it works properly when enabling the panel in runtime.
	public GameObject stageButtonPanel; // Should contain N-amount of buttons.
	public MenuPanelHandler menuPanelHandler;

	private List<StageChoiceButton> buttonList;
	private List<StageData> stages;


	void Awake(){
		buttonList = new List<StageChoiceButton>(stageButtonPanel.GetComponentsInChildren<StageChoiceButton> ());
		stages = new List<StageData>(StageDataController.GetStages ());
	}

	void OnEnable(){
		//InitializeButtonsFromPanel (StageTag.T1v1); // Debug
		InitializeButtonsFromPanel(StageTagHandler.GetTagsFromCurrentPlayers());
		switch (MasterStateController.GetTheMasterData ().stageChoiceType) {
		case StageChoiceType.individualRandom:
			menuPanelHandler.SetCursors (false);
			break;
		case StageChoiceType.masterSingle:
			menuPanelHandler.SetCursors (true);
			break;
		case StageChoiceType.randomPreset:
			MasterStateController.GoToBattle ();
			break;
		}
	}

	public void InitializeButtonsFromPanel(StageTag primaryTag){
		int indexCap;
		if (stages.Count> buttonList.Count) {
			indexCap = buttonList.Count;
		} else {
			indexCap = stages.Count;
		}
		Debug.Log ("Stages found: " + stages.Count + " | Buttons found: " + buttonList.Count);
		Debug.Log ("Index cap: " + indexCap);
		for (int i = 0; i < indexCap; i++) {
			buttonList [i].Initialize (stages [i]);
			buttonList [i].CheckPrimaryTag (primaryTag);
		}
		for (int i = indexCap; i<buttonList.Count;i++){
			buttonList [i].gameObject.SetActive (false);
		}
	}
}
