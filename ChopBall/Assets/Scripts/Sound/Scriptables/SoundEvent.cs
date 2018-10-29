using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundEvent : ScriptableObject {

	private List<SoundEventListener> listeners = new List<SoundEventListener> ();

	public void Raise(SoundInfo info){
		for (int i = listeners.Count - 1; i >= 0; i--) {
			listeners [i].OnEventRaised (info);
		}
	}
	public void RegisterListener(SoundEventListener listener){
		listeners.Add (listener);
	}
	public void UnregisterListener(SoundEventListener listener){
		listeners.Remove (listener);
	}
}
