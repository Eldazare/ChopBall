using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemapCursorButton : _CursorButton {

	private ButtonCommand designatedCommand;
	public delegate void ReturnMapCall(ButtonCommand BC, int playerID);
	private ReturnMapCall returnMapCall;

	public void Initialize(ButtonCommand command, ReturnMapCall rmc){
		designatedCommand = command;
		returnMapCall = rmc;
		gameObject.GetComponentInChildren<Text> ().text = command.ToString ();
	}

	override
	public void Click(int playerID){
		returnMapCall (designatedCommand, playerID);
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hovering above command " + designatedCommand + " remapping button");
	}
}
