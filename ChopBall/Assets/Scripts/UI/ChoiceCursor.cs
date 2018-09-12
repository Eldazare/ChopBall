using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceCursor : MonoBehaviour {

	public int playerID;
	public int pixelBuffer = 10;
	public float movementMultiplier = 100;
	float XMin;
	float XMax;
	float YMin;
	float YMax;
	InputModel model;
	RectTransform rect;
	GraphicRaycaster raycaster;
	PlayerStateData stateData;
	private Vector3 lastPos;

	private List<RaycastResult> results;
	PointerEventData ped;
	_CursorButton foundButton; // universal used in raycast
	_CursorButton hoverButton; // Used in fixed updat
	_CursorButton clickButton; // Used when "submit":ting

	bool lateSubmit = false;
	bool lateCancel = false;

	void Awake(){
		rect = gameObject.GetComponent<RectTransform> ();
		raycaster = gameObject.GetComponentInParent<GraphicRaycaster> ();
		stateData = (PlayerStateData) Resources.LoadAll ("Scriptables/Players/StateData", typeof(PlayerStateData)) [playerID - 1];
		GetBounds ();
		model = new InputModel ();
		lastPos = rect.position;
		ped = new PointerEventData (null);
	}

	void GetBounds(){
		float height = Screen.height;
		float width = Screen.width;
		XMin = 0 + pixelBuffer;
		XMax = width - pixelBuffer;
		YMin = 0 + pixelBuffer;
		YMax = height - pixelBuffer;
	}

	public void GetModel(InputModel model){
		this.model = model;
	}

	void Update(){
		if (!stateData.XYmovementLocked) {
			float newX = rect.position.x + (model.XAxisLeft * Time.deltaTime * movementMultiplier);
			float newY = rect.position.y + (model.YAxisLeft * Time.deltaTime * movementMultiplier);
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
			rect.position = new Vector2 (newX, newY);
		}
		if (lateSubmit == false) {
			if (model.Submit == true) {
				clickButton = RaycastAButton ();
				if (clickButton != null) {
					clickButton.Click (playerID);
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
		
	void FixedUpdate(){
		if (rect.position != lastPos) {
			lastPos = rect.position;
			hoverButton = RaycastAButton ();
			if (hoverButton != null){
				hoverButton.Hover (playerID);
			}
		}
	}


	_CursorButton RaycastAButton(){
		ped.position = rect.position;
		results = new List<RaycastResult> ();
		raycaster.Raycast (ped, results);
		foreach (RaycastResult res in results) {
			foundButton = res.gameObject.GetComponent<_CursorButton> ();
			if (foundButton != null){
				return foundButton;
			}
		}
		return null;
	}
}
