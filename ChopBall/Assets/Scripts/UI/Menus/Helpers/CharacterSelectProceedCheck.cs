using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelectProceedCheck : MonoBehaviour {

	public GameEvent forward;
	private Text[] texts;
	private Image[] image;
	private GameEventListener charProceedListener;


	void Awake(){
		image = GetComponentsInChildren<Image> ();
		texts = GetComponentsInChildren<Text> ();
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
		foreach (Text text in texts) {
			text.enabled = active;
		}
		charProceedListener.enabled = active;
	}

	public void Proceed(){
		forward.Raise ();
	}
}
