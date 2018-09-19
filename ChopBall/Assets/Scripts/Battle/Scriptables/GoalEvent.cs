using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEvent : ScriptableObject {

	private List<GoalEventListener> listeners = new List<GoalEventListener> ();

	public void Raise(GoalData model){
		for (int i = listeners.Count - 1; i >= 0; i--) {
			listeners [i].OnEventRaised (model);
		}
	}
	public void RegisterListener(GoalEventListener listener){
		listeners.Add (listener);
	}
	public void UnregisterListener(GoalEventListener listener){
		listeners.Remove (listener);
	}
}
