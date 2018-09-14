using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCursor : _Cursor {

	protected Vector2 totalDirectionLeft;
	InputEventListener[] listeners;
	protected bool controllerChosen;

	void Awake(){
		Initialize ();
		ResetTotalAxes ();
		listeners = GetComponents<InputEventListener> ();
		controllerChosen = false;
	}


	override
	public void GetModel(InputModel gotModel){
		totalDirectionLeft += gotModel.leftDirection;
		model = gotModel;
		if (controllerChosen == false) {
			if (model.Submit == true) {
				controllerChosen = true;
				EnableOnlyListener (model.playerID-1);
				this.playerID = model.playerID;
			}
		}
	}

	void Update(){
		Movement (totalDirectionLeft);
		ResetTotalAxes ();
		SubmitToClick ();
		CancelCheck ();
	}

	void FixedUpdate(){
		ContinuedHover ();
	}

	protected void ResetTotalAxes(){
		totalDirectionLeft = Vector2.zero;
	}

	protected void EnableOnlyListener(int wantedIndex){
		for (int i = 0; i<listeners.Length;i++) {
			if (i != wantedIndex) {
				listeners [i].enabled = false;
			} else {
				listeners [i].enabled = true;
			}
		}
	}

	protected void EnabelAllListeners(){
		foreach (InputEventListener listener in listeners) {
			listener.enabled = true;
		}
	}
}
