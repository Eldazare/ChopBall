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
		stateData = (PlayerStateData) Resources.LoadAll ("Scriptables/Players/StateData", typeof(PlayerStateData)) [playerID - 1];
	}

	void Update(){
		if (!stateData.XYmovementLocked) {
			Movement (model.leftDirectionalInput);
		}
		SubmitToClick ();
		CancelCheck ();
	}
		
	void FixedUpdate(){
		ContinuedHover ();
	}
}
