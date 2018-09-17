using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class _Cursor : MonoBehaviour {

	public int playerID = -1;
	public int pixelBuffer = 10;
	public float movementMultiplier = 50;
	private float boundsMultiplier = 1;
	protected float XMin;
	protected float XMax;
	protected float YMin;
	protected float YMax;
	protected InputModel model;
	protected RectTransform rect;
	protected GraphicRaycaster raycaster;
	protected Vector3 lastPos;

	protected List<RaycastResult> results;
	protected PointerEventData ped;
	protected _CursorButton foundButton; // universal used in raycast
	protected _CursorButton pastHoverButton;
	protected _CursorButton hoverButton; // Used in fixed updat
	protected _CursorButton clickButton; // Used when "submit":ting

	protected bool lateSubmit = false;
	protected bool lateCancel = false;

	protected void Initialize(){
		rect = gameObject.GetComponent<RectTransform> ();
		raycaster = gameObject.GetComponentInParent<GraphicRaycaster> ();
		GetBounds ();
		model = new InputModel ();
		lastPos = rect.position;
		ped = new PointerEventData (null);		
	}

	protected void GetBounds(){
		float height = Screen.height;
		float width = Screen.width;
		XMin = 0 + pixelBuffer;
		XMax = width - pixelBuffer;
		YMin = 0 + pixelBuffer;
		YMax = height - pixelBuffer;

		boundsMultiplier = height / 100 + width / 100;
		Debug.Log ("Bounds multiplier:" +boundsMultiplier);
	}

	virtual
	public void GetModel(InputModel gotModel){
		this.model = gotModel;
	}

	protected void Movement(Vector2 direction){
		float newX = rect.position.x + (direction.x * Time.deltaTime * movementMultiplier * boundsMultiplier);
		float newY = rect.position.y + (direction.y * Time.deltaTime * movementMultiplier * boundsMultiplier);
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

	protected void SubmitToClick(){
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
	}

	protected void CancelCheck(){
		if (lateCancel == false) {
			if (model.Cancel == true) {
				// TODO: Cancel?
				lateCancel = true;
			}
		} else if (model.Cancel == false) {
			lateCancel = false;
		}
	}

	protected void ContinuedHover(){
		if (rect.position != lastPos) {
			lastPos = rect.position;
			hoverButton = RaycastAButton ();
			if (pastHoverButton != null) {
				if (pastHoverButton != hoverButton) {
					pastHoverButton.OnHoverExit (playerID);
				}
			}
			if (hoverButton != null){
				hoverButton.Hover (playerID);
			}
			pastHoverButton = hoverButton;
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

	public void OnScreenSizeChange(){
		GetBounds ();
	}
}
