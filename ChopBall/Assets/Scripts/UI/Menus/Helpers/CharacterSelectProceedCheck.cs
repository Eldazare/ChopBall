using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectProceedCheck : MonoBehaviour {

	private Image image;
	private Text text;

	void Awake(){
		image = GetComponent<Image> ();
		text = GetComponentInChildren<Text> ();
		CheckProceedState ();
	}
		
	public void CheckProceedState(){
		if (StageTagHandler.CanContinueFromPlayerSelect ()) {
			EnableObject (true);
		} else {
			EnableObject (false);
		}
	}

	private void EnableObject(bool active){
		image.enabled = active;
		text.enabled = active;
	}
}
