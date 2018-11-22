using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GrandModeChooseControlButton : _ControlButton {

	public GrandMode chosenMode;
	public Text nameText;
	public UnityEvent forward;

	void Awake(){
		nameText.text = "Multiplayer: "+chosenMode.ToString ();
	}

	override
	public void OnButtonClick(int playerID){
		MasterStateController.GetTheMasterData ().SetGrandMode (chosenMode);
		forward.Invoke ();
	}
}
