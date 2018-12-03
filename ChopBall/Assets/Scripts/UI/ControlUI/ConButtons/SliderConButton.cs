using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderConButton : _ControlButton {

	public delegate void SliderUpdate(float value);

	public float min;
	public float max;
	public float stepAmount; // number of "clicks" it takes to go from min to max
	public float currentValue;

	public Text numberDisplay;
	public GameObject indicatorChild;
	public InputEventListener inputListener;

	private float rangeMagnitude;
	private float singleStep;
	private SliderUpdate updateCall;
	private RectTransform selfRect;
	private DPosition dpos;

	public void Initialize(SliderUpdate updateCall){
		rangeMagnitude = (max - min);
		this.selfRect = GetComponent<RectTransform> ();
		singleStep = rangeMagnitude / stepAmount;
		this.updateCall = updateCall;
		SetIndicator ();
	}

	private void IncDecSliderValue(bool incDec){
		CheckBounds (incDec);
		SetIndicator ();
		updateCall.Invoke (currentValue);
	}

	public void GetInput(InputModel model){
		Debug.Log ("FFF");
		if (model.D_PadVector.x > 0.5f || model.leftDirectionalInput.x > 0.5f) {
			IncDecSliderValue (true);
		} else if (model.D_PadVector.x < -0.5f || model.leftDirectionalInput.x < -0.5f) {
			IncDecSliderValue (false);
		}
	}

	private void SetIndicator(){
		float percentage = (currentValue - min) / rangeMagnitude;
		percentage -= 0.5f;
		percentage *= selfRect.rect.width;
		indicatorChild.transform.localPosition = new Vector3 (percentage, 0, 0);
		numberDisplay.text = currentValue.ToString ();
	}

	private void CheckBounds(bool incDec){
		if (incDec) {
			currentValue += singleStep;
			if (currentValue > max) {
				currentValue = max;
			}
		} else {
			currentValue -= singleStep;
			if (currentValue < min) {
				currentValue = min;
			}
		}
	}

	override
	public void OnButtonEnter(int playerID){
		inputListener.Event = InputEventController.GetEventByIndex (playerID);
		inputListener.enabled = true;
	}

	override
	public void OnButtonExit(int playerID){
		inputListener.enabled = false;
		GetInput (new InputModel ());
	}

//	override
//	public void OnButtonLeftBumper(int playerID){
//		IncDecSliderValue (false);
//	}
//
//	override
//	public void OnButtonRightBumper(int playerID){
//		IncDecSliderValue (true);
//	}
}
