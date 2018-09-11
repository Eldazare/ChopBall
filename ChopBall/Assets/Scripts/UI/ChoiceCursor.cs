using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceCursor : MonoBehaviour {

	public int playerID;
	float XMin;
	float XMax;
	float YMin;
	float YMax;
	InputModel model;
	RectTransform rect;
	GraphicRaycaster raycaster;

	bool lateSubmit = false;
	bool lateCancel = false;

	void Awake(){
		rect = gameObject.GetComponent<RectTransform> ();
		raycaster = gameObject.GetComponentInParent<GraphicRaycaster> ();
		GetBounds ();
		model = new InputModel ();
	}

	void GetBounds(){
		float height = Screen.width;
		float width = Screen.height;
		XMin = -width / 2;
		XMax = width / 2;
		YMin = -height / 2;
		YMax = height / 2;
	}

	public void GetModel(InputModel model){
		this.model = model;
	}

	void FixedUpdate(){
		float newX = rect.position.x + model.XAxisLeft;
		float newY = rect.position.y + model.YAxisLeft;
		rect.position = new Vector2 (newX, newY);
		if (newX > XMax) {
			newX = XMax;
		}
		if (newX < XMin) {
			newX = XMin;
		} 
		if (newY > YMax) {
			newY = YMax;
		}
		if (newY < YMin) {
			newY = YMin;
		}

		if (lateSubmit == false) {
			if (model.Submit == true) {
				_CursorButton button = RaycastAButton ();
				if (button != null) {
					button.Click (playerID);
				}
				lateSubmit = true;
			}
		} else if (model.Submit == false) {
			lateSubmit = false;
		}

		if (lateCancel == false) {
			if (model.Cancel == true) {
				// TODO: Cancel?
				lateCancel = true;
			}
		} else if (model.Cancel == false) {
			lateCancel = false;
		}
	}


	_CursorButton RaycastAButton(){
		PointerEventData ped = new PointerEventData (null);
		ped.position = rect.position;
		List<RaycastResult> results = new List<RaycastResult> ();
		raycaster.Raycast (ped, results);
		foreach (RaycastResult res in results) {
			_CursorButton button = res.gameObject.GetComponent<_CursorButton> ();
			if (button != null){
				return button;
			}
		}
		return null;
	}
}
