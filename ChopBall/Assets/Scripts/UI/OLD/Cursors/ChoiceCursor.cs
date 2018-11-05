using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceCursor : _Cursor {
	
	protected PlayerStateData stateData;

	override
	protected void Awake(){
		base.Awake ();
		GetComponent<Image> ().color = ((PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData")).playerColors [playerID - 1];
		stateData = PlayerStateController.GetAState(playerID);
	}

	public void CheckActiveState(){
		gameObject.SetActive (stateData.active);
	}

	void Update(){
		Movement (model.leftDirectionalInput, model.Dash);
		ButtonCheck ();
		if (CancelCheck ()) {
			if (stateData.CharacterLocked && stateData.characterChoosing) {
				PlayerStateController.ChooseCharacter (playerID, stateData.characterChoice);
			} else if (raycastIfNull (clickButton) == null) {
				OnCancel.Raise ();
			} else if (raycastIfNull (clickButton).GetType () == typeof(RemapCursorButton)) {
				// Do nothing, Button mapper is mapping button
			}
			else {
				OnCancel.Raise ();
			}
		}
	}
		
	void FixedUpdate(){
		ContinuedHover ();
	}
}
