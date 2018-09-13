using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputTranslator : MonoBehaviour {

	public int controllerNumber;
	public UnityInputEvent UpdateInputs;
	public bool invertYInput = true;
	private string begin;
	private InputStorage customInputs;
	private PlayerStateData stateData;

	void Awake(){
		customInputs = InputStorageController.GetAStorage(controllerNumber);
		if (customInputs.playerNo != controllerNumber) {
			Debug.LogWarning ("Misaligned customInput and controller "+customInputs.playerNo+" and "+controllerNumber);
		}
		/*
		stateData = PlayerStateController.GetAState (controllerNumber);
		if (stateData == null) {
			Debug.LogWarning ("InputTranslator " + controllerNumber + " didn't find playerState");
		}
		*/
	}

	void Update(){
		InputModel model = new InputModel ();
		model.playerID = controllerNumber;
		model.XAxisLeft = Input.GetAxisRaw (customInputs.XAxisLeft);
		model.XAxisRight = Input.GetAxisRaw (customInputs.XAxisRight);
		if (invertYInput) {
			model.YAxisLeft = -Input.GetAxisRaw (customInputs.YAxisLeft);
			model.YAxisRight = -Input.GetAxisRaw (customInputs.YAxisRight);
		} else {
			model.YAxisLeft = Input.GetAxisRaw (customInputs.YAxisLeft);
			model.YAxisRight = Input.GetAxisRaw (customInputs.YAxisRight);		
		}

		model.PaddleLeft = Input.GetKey (customInputs.PaddleLeft);
		model.PaddleRight = Input.GetKey (customInputs.PaddleRight);
		model.Dash = Input.GetKey (customInputs.Dash);
		model.Submit = Input.GetKeyDown (customInputs.Submit);
		model.Cancel = Input.GetKeyDown (customInputs.Cancel);
		UpdateInputs.Invoke (model);
	}

	public void FinalCall(){
		UpdateInputs.Invoke (new InputModel ());
	}
}
