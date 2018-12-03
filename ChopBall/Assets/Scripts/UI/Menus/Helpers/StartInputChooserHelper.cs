using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartInputChooserHelper : MonoBehaviour {

	public _ControlCursor cursor;
	public GameEvent forward;

	public void GetInput(InputModel model){
		if (model.Start) {
			cursor.GetComponent<InputEventListener> ().Event = InputEventController.GetEventByIndex(model.playerID);
			cursor.playerID = model.playerID;
			forward.Raise ();
		}
	}
}
