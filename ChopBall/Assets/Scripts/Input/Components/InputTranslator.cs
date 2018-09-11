using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputTranslator : MonoBehaviour {

	public int controllerNumber;
	public UnityInputEvent UpdateInputs;
	private string begin;
	private InputStorage customInputs;
	private PlayerStateData stateData;

	void Awake(){
		begin = "P" + controllerNumber + "_";
		//TODO:
		customInputs = (InputStorage) Resources.LoadAll ("Scriptables/Input/ButtonStorage")[controllerNumber-1];
		if (customInputs.playerNo != controllerNumber) {
			Debug.LogWarning ("Misaligned customInput and controller "+customInputs.playerNo+" and "+controllerNumber);
		}
		stateData = (PlayerStateData)Resources.LoadAll ("Scriptables/Players/StateData") [controllerNumber - 1];
		if (stateData == null) {
			Debug.LogWarning ("InputTranslator " + controllerNumber + " didn't find playerState");
		}
	}

	void Update(){
		InputModel model = new InputModel ();
		if (!stateData.XYmovementLocked) {
			model.XAxisLeft = Input.GetAxisRaw (begin + "Horizontal_Left");
			model.YAxisLeft = Input.GetAxisRaw (begin + "Vertical_Left");
		}
		model.XAxisRight = Input.GetAxisRaw (begin + "Horizontal_Right");
		model.YAxisRight = Input.GetAxisRaw (begin + "Vertical_Right");
		/*
		model.PaddleLeft = Input.GetButton (begin + "Paddle_Left");
		model.PaddleRight = Input.GetButton (begin + "Paddle_Right");
		model.Submit = Input.GetButtonDown (begin + "Submit");
		model.Cancel = Input.GetButtonDown (begin + "Cancel");
		*/
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
