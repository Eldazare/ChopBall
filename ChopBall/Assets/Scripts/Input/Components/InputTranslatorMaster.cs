﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTranslatorMaster : MonoBehaviour {

	// TODO: Maybe store bool data somewhere and read it for Translators?
	public bool translateDpad = false;


	private bool CheckForControllers = false;
	private InputTranslator[] translators;
	private bool[] connectedControllers = { false, false, false, false };
	public static bool[] modelsPut = { false, false, false, false }; // Preferably get only

	public void StartChecking (){
		CheckForControllers = true;
		StartCoroutine (CheckControllers ());
	}

	public void CheckOnce(){
		string[] JoyNames = Input.GetJoystickNames ();
		Debug.Log (JoyNames.Length);
		for (int i = 0; i < JoyNames.Length; i++) {
			if (!string.IsNullOrEmpty (JoyNames [i])) {
				if (!connectedControllers [i]) {
					if (!modelsPut [i]) {
						Debug.Log ("Trying: " + JoyNames [i]);
						if (InputStorageController.SetModelToStorage ((i + 1), JoyNames [i])) {
							modelsPut [i] = true;
							Debug.Log ("Joystick " + i + " Connected");
							connectedControllers [i] = true;
							translators [i].enabled = true;
							translators [i].SetTranslateDPad(translateDpad);
						}
					} else {
						translators [i].enabled = true;
						connectedControllers [i] = true;
						translators [i].SetTranslateDPad(translateDpad);
					}
				} 
			} else {
				if (connectedControllers[i] == true) {
					Debug.Log ("Joystick " + i + " Disconnected");
					translators [i].FinalCall ();
					translators [i].enabled = false;
					connectedControllers [i] = false;
				}
			}
		}
	}

	public void StopChecking(){
		CheckForControllers = false;
	}

	void Awake(){
		translators = gameObject.GetComponents<InputTranslator> ();
		//CheckUntilFound ();
		//StartChecking ();
		CheckOnce();
	}

	private IEnumerator CheckControllers(){
		while (CheckForControllers) {
			yield return new WaitForSecondsRealtime (2f);
			CheckOnce ();
		}
	}

	private IEnumerator CheckUntilFound(){
		while (Input.GetJoystickNames ().Length == 0) {
			yield return new WaitForSecondsRealtime (2f);
			CheckOnce ();
		}
	}
}
