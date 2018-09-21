using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTranslatorMaster : MonoBehaviour {

	// TODO: Maybe store bool data somewhere and read it for Translators?

	private bool CheckForControllers = false;
	private InputTranslator[] translators;
	private bool[] connectedControllers = { false, false, false, false };

	public void StartChecking (){
		CheckForControllers = true;
		StartCoroutine (CheckControllers ());
	}

	public void CheckOnce(){
		string[] JoyNames = Input.GetJoystickNames ();
		for (int i = 0; i < JoyNames.Length; i++) {
			if (!string.IsNullOrEmpty (JoyNames [i])) {
				if (connectedControllers [i] == false) {
					Debug.Log ("Trying: "+ JoyNames [i]);
					if (InputStorageController.SetModelToStorage ((i + 1), JoyNames [i])) {
						connectedControllers [i] = true;
						Debug.Log ("Joystick " + i + " Connected");
						translators [i].enabled = true;
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
