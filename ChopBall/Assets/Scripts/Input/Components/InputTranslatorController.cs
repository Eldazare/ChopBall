using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTranslatorController : MonoBehaviour {

	private bool CheckForControllers = false;
	private InputTranslator[] translators;

	// Public Scriptable 

	public void StartChecking (){
		CheckForControllers = true;
		StartCoroutine (CheckControllers ());
	}

	public void StopChecking(){
		CheckForControllers = false;
	}

	void Awake(){
		translators = gameObject.GetComponents<InputTranslator> ();
		StartChecking ();
	}

	private IEnumerator CheckControllers(){
		while (CheckForControllers) {
			yield return new WaitForSecondsRealtime (2f);
			// Scriptable: Dictionary
			string[] JoyNames = Input.GetJoystickNames ();
			for (int i = 0; i < JoyNames.Length; i++) {
				if (!string.IsNullOrEmpty (JoyNames [i])) {
					Debug.Log ("Joystick "+i+" Connected");
					translators [i].enabled = true;
				} else {
					Debug.Log ("Joystick "+i+" Disconnected");
					translators [i].FinalCall ();
					translators [i].enabled = false;
				}
			}
		}
	}
}
