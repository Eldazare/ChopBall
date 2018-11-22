using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartInputChooserHelper : MonoBehaviour {

	public List<InputEvent> inputEvents;
	public _ControlCursor cursor;
	public UnityEvent forward;

	public void GetInput(InputModel model){
		if (model.Start) {
			cursor.GetComponent<InputEventListener> ().Event = inputEvents [model.playerID - 1];
			forward.Invoke ();
		}
	}
}
