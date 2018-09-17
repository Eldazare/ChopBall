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
		Text text = gameObject.GetComponentInChildren<Text> ();
		if (text != null) {
			text.text = command.ToString ();
		}
	}

	override
	public void Click(int playerID){
		// Do nothing. Just believe that the mapper input translator is active.
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hovering above command " + designatedCommand + " remapping button");
		returnMapCall (designatedCommand, playerID);
	}
}
