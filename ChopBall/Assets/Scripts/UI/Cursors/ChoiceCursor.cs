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
		stateData = PlayerStateController.GetAState(playerID);
	}

	void Update(){
		Movement (model.leftDirectionalInput, model.Dash);
		ButtonCheck ();
		if (CancelCheck ()) {
			if (stateData.CharacterLocked && stateData.characterChoosing) {
				PlayerStateController.ChooseCharacter (playerID, stateData.characterChoice);
			} else {
				OnCancel.Raise ();
			}
		}
	}
		
	void FixedUpdate(){
		ContinuedHover ();
	}
}
