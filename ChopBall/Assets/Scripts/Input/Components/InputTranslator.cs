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
	float PaddleStrValue;

	void Update(){
		InputModel model = new InputModel ();
		model.playerID = controllerNumber;
		model.XAxisLeft = Input.GetAxisRaw (customInputs.XAxisLeft);
		model.XAxisRight = Input.GetAxisRaw (customInputs.XAxisRight);
		model.leftDirection.x = Input.GetAxisRaw (customInputs.XAxisLeft);
		model.rightDirection.x = Input.GetAxisRaw (customInputs.XAxisRight);
		if (invertYInput) {
			model.YAxisLeft = -Input.GetAxisRaw (customInputs.YAxisLeft);
			model.YAxisRight = -Input.GetAxisRaw (customInputs.YAxisRight);
			model.leftDirection.y = -Input.GetAxisRaw (customInputs.YAxisLeft);
			model.rightDirection.y = -Input.GetAxisRaw (customInputs.YAxisRight);
		} else {
			model.YAxisLeft = Input.GetAxisRaw (customInputs.YAxisLeft);
			model.YAxisRight = Input.GetAxisRaw (customInputs.YAxisRight);	
			model.leftDirection.y = -Input.GetAxisRaw (customInputs.YAxisLeft);
			model.rightDirection.y = -Input.GetAxisRaw (customInputs.YAxisRight);
		}
		DeadZoneCheck (model, customInputs.deadZoneLeft, customInputs.deadZoneRight);

		model.PaddleLeft = Input.GetKey (customInputs.PaddleLeft);
		model.PaddleRight = Input.GetKey (customInputs.PaddleRight);
		model.Dash = Input.GetKey (customInputs.Dash);
		model.Submit = Input.GetKey (customInputs.Submit);
		model.Cancel = Input.GetKey (customInputs.Cancel);
		UpdateInputs.Invoke (model);
	}

	private void DeadZoneCheck(InputModel nearFinishedModel, float deadZoneLeft, float deadZoneRight){
		//Deprecated
		float leftTotal = Mathf.Abs(nearFinishedModel.XAxisLeft) + Mathf.Abs(nearFinishedModel.YAxisLeft);
		float rightTotal = Mathf.Abs(nearFinishedModel.XAxisRight) + Mathf.Abs(nearFinishedModel.YAxisRight);
		if (leftTotal < deadZoneLeft) {
			nearFinishedModel.XAxisLeft = 0;
			nearFinishedModel.YAxisLeft = 0;
		}
		if (rightTotal < deadZoneRight) {
			nearFinishedModel.XAxisRight = 0;
			nearFinishedModel.YAxisRight = 0;
		}

		// Real
		nearFinishedModel.leftDirection = IndividualDeadzones (nearFinishedModel.leftDirection, deadZoneLeft);
		nearFinishedModel.rightDirection = IndividualDeadzones (nearFinishedModel.rightDirection, deadZoneRight);
	}

	private Vector2 IndividualDeadzones(Vector2 dir, float deadZone){
		if (dir.magnitude < deadZone)
			return Vector2.zero;
		else
			return dir.normalized * ((dir.magnitude - deadZone) / (1 - deadZone));
	}

	public void FinalCall(){
		UpdateInputs.Invoke (new InputModel ());
	}
}
