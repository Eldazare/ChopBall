using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class InputEvent : ScriptableObject {

	private List<InputEventListener> listeners = new List<InputEventListener> ();

	public void Raise(InputModel model){
		for (int i = listeners.Count - 1; i >= 0; i--) {
			listeners [i].OnEventRaised (model);
		}
	}
	public void RegisterListener(InputEventListener listener){
		listeners.Add (listener);
	}
	public void UnregisterListener(InputEventListener listener){
		listeners.Remove (listener);
	}
}
