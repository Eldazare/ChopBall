using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class InputStorage : ScriptableObject {

	public bool active = false;
	public int playerNo;
	public bool UseJoystickLeft = true;
	public bool UseJoystickRight = true;
	public KeyCode PaddleLeft;
	public KeyCode PaddleRight;
	public KeyCode Dash;
	public KeyCode Submit; // For menus and such
	public KeyCode Cancel; // -||-

	public void SetDefaultButtons(){
		PaddleLeft = (KeyCode)Enum.Parse (typeof(KeyCode), "Joystick" + playerNo + "Button5");
		PaddleRight = (KeyCode)Enum.Parse (typeof(KeyCode), "Joystick" + playerNo + "Button6");
		Dash = (KeyCode)Enum.Parse (typeof(KeyCode), "Joystick" + playerNo + "Button7");
		Submit = (KeyCode)Enum.Parse (typeof(KeyCode), "Joystick" + playerNo + "Button0");
		Cancel = (KeyCode)Enum.Parse (typeof(KeyCode), "Joystick" + playerNo + "Button1");
	}
}
