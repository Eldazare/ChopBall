using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ButtonCommand {None, PaddleLeft, PaddleRight, Dash, Block, Submit, Cancel, Start, Select}

[CreateAssetMenu]
public class InputStorage : ScriptableObject {

	public string usedModelName;
	public string XAxisLeft;
	public string YAxisLeft;
	public string XAxisRight;
	public string YAxisRight;
	public float deadZoneLeft;
	public float deadZoneRight;

	public bool active = false;
	public int playerNo;
	public KeyCode PaddleLeft;
	public KeyCode PaddleRight;
	public KeyCode Block;
	public KeyCode Dash;
	public KeyCode Submit; // For menus and such
	public KeyCode Cancel; // -||-
	public KeyCode Start; // For menus and such
	public KeyCode Select; // -||-

	public void ChangeAButton(ButtonCommand command, KeyCode newButton){
		switch (command) {
		case ButtonCommand.PaddleLeft:
			PaddleLeft = newButton;
			break;
		case ButtonCommand.PaddleRight:
			PaddleRight = newButton;
			break;
		case ButtonCommand.Dash:
			Dash = newButton;
			break;
		case ButtonCommand.Block:
			Block = newButton;
			break;
		case ButtonCommand.Submit:
			Submit = newButton;
			break;
		case ButtonCommand.Cancel:
			Cancel = newButton;
			break;
		case ButtonCommand.Start:
			Start = newButton;
			break;
		case ButtonCommand.Select:
			Select = newButton;
			break;
		}
	}

	public void ReadModel(ControllerModel model){
		XAxisLeft = GetAxisString (model.XAxisLeft);
		YAxisLeft= GetAxisString (model.YAxisLeft);
		XAxisRight = GetAxisString (model.XAxisRight);
		YAxisRight = GetAxisString (model.YAxisRight);
		deadZoneLeft = model.deadZone;
		deadZoneRight = model.deadZone;
		if (usedModelName != model.ControllerName) {
			usedModelName = model.ControllerName;
			ReadModelDefaultButtons(model);
		}

	}

	public void ReadDefaultButtonsFromCurrentModel(){
		if (!string.IsNullOrEmpty(usedModelName)){
			ReadModelDefaultButtons(ControllerModelController.GetControllerModel (usedModelName));
		}
	}

	private void ReadModelDefaultButtons(ControllerModel model){
		PaddleLeft = GetButtonCode (model.PaddleLeft);
		PaddleRight = GetButtonCode (model.PaddleRight);
		Dash = GetButtonCode (model.Dash);
		Block = GetButtonCode (model.Block);
		Submit = GetButtonCode (model.Submit);
		Cancel = GetButtonCode (model.Cancel);
		Start = GetButtonCode (model.Start);
		Select = GetButtonCode (model.Select);
	}

	private KeyCode GetButtonCode(int buttonNumber){
		return (KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + playerNo + "Button" + buttonNumber);
	}

	// ACCORDANCE: Defined in Unity's InputManager
	private string GetAxisString(int axisNumber){
		return "P" + playerNo + "Axis" + axisNumber;
	}
}
