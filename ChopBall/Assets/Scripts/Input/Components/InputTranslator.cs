﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputTranslator : MonoBehaviour {

	public int controllerNumber;
	public UnityInputEvent UpdateInputs;
	public bool invertYInput = true;
	private bool translateDpad = false;
	private string begin;
	private InputStorage customInputs;
	private PlayerStateData stateData;
	private InputBaseData baseData;
	private Vector2 previousDirectionalInputLeft;
	private Vector2 previousDirectinalInputRight;

	private Vector2 smoothVelocityLeft;
	private Vector2 smoothVelocityRight;
	private InputModel model;

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
		model = new InputModel ();
		/*
		stateData = PlayerStateController.GetAState (controllerNumber);
		if (stateData == null) {
			Debug.LogWarning ("InputTranslator " + controllerNumber + " didn't find playerState");
		}
		*/
	}

	void Update(){
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
		if (translateDpad) {
			model.D_PadVector.x = Input.GetAxisRaw (customInputs.D_PadAxisX);
			model.D_PadVector.y = Input.GetAxisRaw (customInputs.D_PadAxisY);
		}
		model.Strike = Input.GetKey (customInputs.Strike);
		model.Dash = (Input.GetKey (customInputs.Dash) || Input.GetAxisRaw(customInputs.DashAxis)>baseData.triggerTreshold);
		model.Block = (Input.GetKey (customInputs.Block) || Input.GetAxisRaw(customInputs.BlockAxis)>baseData.triggerTreshold);
		model.Submit = Input.GetKey (customInputs.Submit);
		model.Cancel = Input.GetKey (customInputs.Cancel);
		model.Start = Input.GetKey (customInputs.Start);
		model.Select = Input.GetKey (customInputs.Select);
		UpdateInputs.Invoke (model);
	}

	private void DeadZoneCheck(InputModel nearFinishedModel, float deadZoneLeft, float deadZoneRight){
		nearFinishedModel.leftDirectionalInput = IndividualDeadzonesAndCap (nearFinishedModel.leftDirectionalInput, deadZoneLeft, previousDirectionalInputLeft, ref smoothVelocityLeft);
		nearFinishedModel.rightDirectionalInput = IndividualDeadzonesAndCap (nearFinishedModel.rightDirectionalInput, deadZoneRight, previousDirectinalInputRight, ref smoothVelocityRight);
	}

	private Vector2 IndividualDeadzonesAndCap(Vector2 inputVector, float deadZone, Vector2 previousInput, ref Vector2 smoothVelocity){
		Vector2 smoothedVector;
		if (inputVector.magnitude < deadZone)
			smoothedVector = Vector2.zero;
		else {
			smoothedVector = inputVector.normalized * ((inputVector.magnitude - deadZone) / (1 - deadZone));
			if (smoothedVector.magnitude > 1) {
				smoothedVector.Normalize ();
			}
		}
		return SuperSmooth(smoothedVector, previousInput, ref smoothVelocity);
	}

	private Vector2 SuperSmooth(Vector2 inputVector, Vector2 previousInput, ref Vector2 smoothVelocity){
		

		float gravityMultiplier = (inputVector == Vector2.zero) ? baseData.gravityModifier : 0f;

		Vector2 sensitivityInput = inputVector.normalized * baseData.sensitivityCurve.Evaluate(inputVector.magnitude);
		float inputDistanceDelta = Mathf.Abs((sensitivityInput - previousInput).magnitude);

		inputVector.x = Mathf.SmoothDamp(previousInput.x, sensitivityInput.x, ref smoothVelocity.x, baseData.smoothingCurve.Evaluate(sensitivityInput.magnitude) * baseData.inputSmoothing * baseData.velocitySmoothingCurve.Evaluate(inputDistanceDelta) * (1 - gravityMultiplier));
		inputVector.y = Mathf.SmoothDamp(previousInput.y, sensitivityInput.y, ref smoothVelocity.y, baseData.smoothingCurve.Evaluate(sensitivityInput.magnitude) * baseData.inputSmoothing * baseData.velocitySmoothingCurve.Evaluate(inputDistanceDelta) * (1 - gravityMultiplier));

		return inputVector;
	}

	public void SetTranslateDPad(bool isTranslated){
		this.translateDpad = isTranslated;
	}

	public void FinalCall(){
		UpdateInputs.Invoke (new InputModel ());
	}
}
