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
	private InputBaseData baseData;
	private Vector2 previousDirectionalInputLeft;
	private Vector2 previousDirectinalInputRight;

	private Vector2 smoothVelocityLeft;
	private Vector2 smoothVelocityRight;

	void Awake(){
		customInputs = InputStorageController.GetAStorage(controllerNumber);
		if (customInputs.playerNo != controllerNumber) {
			Debug.LogWarning ("Misaligned customInput and controller "+customInputs.playerNo+" and "+controllerNumber);
		}
		baseData = (InputBaseData)Resources.Load ("Scriptables/_BaseDatas/InputBaseData", typeof(InputBaseData));
		if (baseData == null) {
			Debug.LogError ("InputBaseData not found.");
		}
		previousDirectionalInputLeft = Vector2.zero;
		previousDirectinalInputRight = Vector2.zero;
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
		model.leftDirectionalInput.x = Input.GetAxisRaw (customInputs.XAxisLeft);
		model.rightDirectionalInput.x = Input.GetAxisRaw (customInputs.XAxisRight);
		if (invertYInput) {
			model.leftDirectionalInput.y = -Input.GetAxisRaw (customInputs.YAxisLeft);
			model.rightDirectionalInput.y = -Input.GetAxisRaw (customInputs.YAxisRight);
		} else {	
			model.leftDirectionalInput.y = -Input.GetAxisRaw (customInputs.YAxisLeft);
			model.rightDirectionalInput.y = -Input.GetAxisRaw (customInputs.YAxisRight);
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
		nearFinishedModel.leftDirectionalInput = IndividualDeadzonesAndCap (nearFinishedModel.leftDirectionalInput, deadZoneLeft, previousDirectionalInputLeft, ref smoothVelocityLeft);
		nearFinishedModel.rightDirectionalInput = IndividualDeadzonesAndCap (nearFinishedModel.rightDirectionalInput, deadZoneRight, previousDirectinalInputRight, ref smoothVelocityRight);
	}

	private Vector2 IndividualDeadzonesAndCap(Vector2 inputVector, float deadZone, Vector2 previousInput, ref Vector2 smoothVelocity){
		if (inputVector.magnitude < deadZone)
			return Vector2.zero;
		else {
			Vector2 smoothedDir = inputVector.normalized * ((inputVector.magnitude - deadZone) / (1 - deadZone));
			if (smoothedDir.magnitude > 1) {
				smoothedDir.Normalize ();
			}
			//return smoothedDir;
			return SuperSmooth(smoothedDir, previousInput, ref smoothVelocity);
		}
	}

	private Vector2 SuperSmooth(Vector2 inputVector, Vector2 previousInput, ref Vector2 smoothVelocity){
		

		//gravityMultiplier = (input == Vector2.zero) ? gravity : 0f;

		Vector2 sensitivityInput = inputVector.normalized * baseData.sensitivityCurve.Evaluate(inputVector.magnitude);
		float inputDistanceDelta = Mathf.Abs((sensitivityInput - previousInput).magnitude);

		inputVector.x = Mathf.SmoothDamp(previousInput.x, sensitivityInput.x, ref smoothVelocity.x, baseData.smoothingCurve.Evaluate(sensitivityInput.magnitude) * baseData.inputSmoothing * baseData.velocitySmoothingCurve.Evaluate(inputDistanceDelta)); //* (1 - gravityMultiplier));
		inputVector.y = Mathf.SmoothDamp(previousInput.y, sensitivityInput.y, ref smoothVelocity.y, baseData.smoothingCurve.Evaluate(sensitivityInput.magnitude) * baseData.inputSmoothing * baseData.velocitySmoothingCurve.Evaluate(inputDistanceDelta)); //* (1 - gravityMultiplier));

		return inputVector;
	}

	public void FinalCall(){
		UpdateInputs.Invoke (new InputModel ());
	}
}
