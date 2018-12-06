using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class InputEventController {

	private static InputEvent[] inputEvents;

	private static void LoadInputEvents(){
		if (inputEvents == null) {
			inputEvents = Resources.LoadAll ("Scriptables/Input/Events/", typeof(InputEvent)).Cast<InputEvent>().ToArray ();
		}
	}

	public static InputEvent[] GetAllEvents(){
		LoadInputEvents ();
		return inputEvents;
	}

	public static InputEvent GetEventByIndex(int playerID){
		LoadInputEvents ();
		return inputEvents [playerID - 1];
	}
}
