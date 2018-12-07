using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GrandModeChooseControlButton : _ControlButton {

	public GrandMode chosenMode;
	public Text nameText;
	public GameEvent forward;

	void Awake(){
		selectSoundPath = SoundPathController.GetPath ("Select");
		nameText.text = "Multiplayer: "+chosenMode.ToString ();
	}

	override
	public void OnButtonClick(int playerID){
		FMODUnity.RuntimeManager.PlayOneShot (selectSoundPath);
		MasterStateController.GetTheMasterData ().SetGrandMode (chosenMode);
		forward.Raise ();
	}
}
