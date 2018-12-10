using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeControlButton : _ControlButton {

	public Text buttonText;
	public GameEvent forward;

	private int modeIndex;
	private Text descriptionText;

	public void Initialize(int modeIndex, Text descriptionText){
		selectSoundPath = SoundPathController.GetPath ("Select");
		this.modeIndex = modeIndex;
		buttonText.text = QuickModeBPController.GetNamePerIndex (modeIndex);
		this.descriptionText = descriptionText;
	}

	override
	public void OnButtonClick(int playerID){
		FMODUnity.RuntimeManager.PlayOneShot (selectSoundPath);
		ChooseGamemode ();
		forward.Raise ();
	}

	override
	public void OnButtonEnter(int playerID){
		descriptionText.text = QuickModeBPController.GetDescriptionPerIndex (modeIndex);
	}

	private void ChooseGamemode(){
		MasterStateController.GetTheMasterData ().quickBpIndex = modeIndex;
	}
}
