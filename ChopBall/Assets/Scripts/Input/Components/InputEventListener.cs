using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputEventListener : MonoBehaviour {

	public InputEvent Event; // Assign which player's inputs you want to listen
	public UnityInputEvent Response;

	private void OnEnable(){
		Event.RegisterListener (this);
	}

	private void OnDisable(){
		Event.UnregisterListener (this);
	}

	public void OnEventRaised(InputModel model){
		Response.Invoke (model);
	}
}
