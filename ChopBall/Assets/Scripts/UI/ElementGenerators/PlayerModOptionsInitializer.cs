using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModOptionsInitializer : MonoBehaviour {

	void Awake(){
		SliderConButton[] sliders = GetComponentsInChildren<SliderConButton> ();
		sliders [0].Initialize (0.1f, 3.0f, PlayerChooseModifiers.movespeedMod, UpdateMovespeed);
	}

	private void UpdateMovespeed(float amount){
		PlayerChooseModifiers.movespeedMod = amount;
	}

}
