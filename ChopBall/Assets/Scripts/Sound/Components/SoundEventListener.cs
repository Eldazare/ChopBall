using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEventListener : MonoBehaviour {

	public SoundEvent Event;
	public UnitySoundEvent Response;

	private void OnEnable(){
		Event.RegisterListener (this);
	}

	private void OnDisable(){
		Event.UnregisterListener (this);
	}

	public void OnEventRaised(SoundInfo info){
		Response.Invoke (info);
	}
}
