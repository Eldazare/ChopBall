using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenuPanel : MonoBehaviour {

	void Awake(){
		SliderConButton[] sliders = GetComponentsInChildren<SliderConButton> ();
		sliders [0].Initialize (SetMasterVolume);
		sliders [1].Initialize (SetMasterVolume);
		sliders [2].Initialize (SetMasterVolume);
	}

	public void SetMasterVolume(float value){
		Debug.Log ("Value set to " + value);
		// TODO
	}

}
