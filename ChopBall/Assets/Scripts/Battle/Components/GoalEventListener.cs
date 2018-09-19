using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalEventListener : MonoBehaviour {

	public GoalEvent Event; // Assign which player's inputs you want to listen
	public UnityGoalEvent Response;

	private void OnEnable(){
		Event.RegisterListener (this);
	}

	private void OnDisable(){
		Event.UnregisterListener (this);
	}

	public void OnEventRaised(GoalData model){
		Response.Invoke (model);
	}
}
