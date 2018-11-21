using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeControlButton : _ControlButton {

	public int modeIndex;
	public Text buttonText;
	public Text descriptionText;

	void Start(){
		buttonText.text = QuickModeBPController.GetNamePerIndex (modeIndex);
	}

	public void Initialize(int modeIndex){
		this.modeIndex = modeIndex;
	}

	override
	public void OnButtonClick(int playerID){
		ChooseGamemode ();
	}

	override
	public void OnButtonEnter(int playerID){
		descriptionText.text = QuickModeBPController.GetDescriptionPerIndex (modeIndex);
	}

	private void ChooseGamemode(){
		MasterStateController.GetTheMasterData ().quickBpIndex = modeIndex;
	}
}
