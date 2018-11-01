using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CharSelectMenuButtonInitializer : MonoBehaviour {

	public List<BlueprintChangeButton> blueprintButtons;
	public Text bpDescription;

	private MasterStateData masterState;
	private QuickModeBlueprint[] quickBpList;

	void Awake(){
		masterState = MasterStateController.GetTheMasterData ();
		quickBpList = QuickModeBPController.GetAllBPs ();
		blueprintButtons [0].Initialize (IncDecGrandMode);
		blueprintButtons [1].Initialize (IncDecPreset);
	}

	void OnEnable(){
		blueprintButtons [0].SetString (masterState.mode.ToString ());
		SetPreset ();
	}

	void OnDisable(){
		if (masterState.quickBpIndex != -1){
			masterState.battleModeBlueprint = quickBpList [masterState.quickBpIndex].GetBlueprint ();
		}
	}

	private int CheckIndex(int currentIndex, int length, bool incDec){
		if (incDec) {
			currentIndex += 1;
		} else {
			currentIndex -= 1;
		}
		if (currentIndex >= length) {
			return 0;
		} else if (currentIndex < 0) {
			return length - 1;
		}
		return currentIndex;
	}

	private void IncDecGrandMode(bool incDec){
		int nextIndex = CheckIndex ((int)masterState.mode, Enum.GetValues (typeof(GrandMode)).Length, incDec);
		masterState.SetGrandMode ((GrandMode)nextIndex);
		blueprintButtons [0].SetString (masterState.mode.ToString ());
	}

	private void IncDecPreset(bool incDec){
		int nextIndex = CheckIndex(masterState.quickBpIndex, quickBpList.Length, incDec);
		masterState.quickBpIndex = nextIndex;
		blueprintButtons [1].SetString (quickBpList [nextIndex].GetName ());
		bpDescription.text = quickBpList [nextIndex].GetDescription ();
	}

	private void SetPreset(){
		if (masterState.quickBpIndex != -1) {
			blueprintButtons [1].SetString (quickBpList [masterState.quickBpIndex].GetName ());
			bpDescription.text = quickBpList [masterState.quickBpIndex].GetDescription ();
		} else {
			blueprintButtons [1].SetString ("Custom");
			bpDescription.text = ("Check advanced menu for more info");
		}
	}
}
