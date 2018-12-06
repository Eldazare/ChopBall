using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenuPanel : MonoBehaviour {

	public float min = 0;
	public float max = 100;
	private float current = 1.0f;

	void Awake(){
		SliderConButton[] sliders = GetComponentsInChildren<SliderConButton> ();
		sliders [0].Initialize (min, max, current, SetMasterVolume);
		sliders [1].Initialize (min, max, current, SetMasterVolume);
		sliders [2].Initialize (min, max, current, SetMasterVolume);
	}

	public void SetMasterVolume(float value){
		Debug.Log ("Value set to " + value);
		// TODO
	}
}
