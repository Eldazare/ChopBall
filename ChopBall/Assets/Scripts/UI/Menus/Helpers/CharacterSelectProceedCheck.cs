using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelectProceedCheck : MonoBehaviour {

	public UnityEvent proceed;
	private Image[] image;
	private Text text;
	private GameEventListener charProceedListener;


	void Awake(){
		image = GetComponentsInChildren<Image> ();
		text = GetComponentInChildren<Text> ();
		charProceedListener = GetComponent<GameEventListener> ();
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
		foreach (var imag in image) {
			imag.enabled = active;
		}
		text.enabled = active;
		charProceedListener.enabled = active;
	}

	public void Proceed(){
		proceed.Invoke ();
	}
}
