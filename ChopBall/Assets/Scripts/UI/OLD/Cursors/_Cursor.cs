using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class _Cursor : MonoBehaviour {

	public int playerID = -1;
	public GameEvent OnCancel;

	protected float XMin;
	protected float XMax;
	protected float YMin;
	protected float YMax;
	protected float boundsMultiplier;
	protected InputModel model;
	protected RectTransform rect;
	protected GraphicRaycaster raycaster;
	protected Vector3 lastPos;

	protected CursorBaseData baseData;
	protected int pixelBuffer;
	protected float movementMultiplier;

	protected List<RaycastResult> results;
	protected PointerEventData ped;
	protected _CursorButton foundButton; // universal used in raycast
	protected _CursorButton pastHoverButton;
	protected _CursorButton hoverButton; // Used in fixed update
	protected _CursorButton clickButton; // Used when "submit":ting

	protected bool lateSubmit = false;
	protected bool lateCancel = false;
	protected bool latePaddleRight = false;
	protected bool laterPaddleLeft = false;

	protected virtual void Awake(){
		baseData = (CursorBaseData) Resources.Load ("Scriptables/_BaseDatas/CursorBaseData", typeof(CursorBaseData));
		movementMultiplier = baseData.movespeedMultiplier;
		pixelBuffer = baseData.pixelBuffer;
		rect = gameObject.GetComponent<RectTransform> ();
		raycaster = gameObject.GetComponentInParent<GraphicRaycaster> ();
		GetBounds ();
		model = new InputModel ();
		lastPos = rect.position;
		ped = new PointerEventData (null);		
	}

	void OnEnable(){
		lateSubmit = true;
		lateCancel = true;
		latePaddleRight = true;
		laterPaddleLeft = true;
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

	protected void Movement(Vector2 direction,bool dash){
		float dashMultiplier = 1;
		if (dash) {
			dashMultiplier = baseData.dashMovespeedMultiplier;
		}
		float newX = rect.position.x + (direction.x * Time.deltaTime * movementMultiplier * boundsMultiplier * dashMultiplier);
		float newY = rect.position.y + (direction.y * Time.deltaTime * movementMultiplier * boundsMultiplier * dashMultiplier);
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

	protected _CursorButton ButtonCheck(){
		clickButton = null;
		if (IsButtonDown (out lateSubmit, lateSubmit, model.Submit)) {
			clickButton = raycastIfNull(clickButton);
			if (clickButton != null) {
				clickButton.Click (playerID);
			}
		}

		if (IsButtonDown(out latePaddleRight, latePaddleRight, model.Strike)){
			clickButton = raycastIfNull(clickButton);
			if (clickButton != null){
				clickButton.OnClickRight();
			}
		}

		if (IsButtonDown (out laterPaddleLeft, laterPaddleLeft, model.Dash)) {
			clickButton = raycastIfNull (clickButton);
			if (clickButton != null){
				clickButton.OnClickLeft();
			}
		}
		return clickButton;
	}

	protected bool CancelCheck(){
		return IsButtonDown (out lateCancel, lateCancel, model.Cancel);
	}

	protected bool IsButtonDown(out bool lateButtonRet, bool lateButton, bool currentButton){
		if (!lateButton) {
			if (currentButton) {
				lateButtonRet = true;
				return true;
			}
			lateButtonRet = false;
		} else if (!currentButton) {
			lateButtonRet = false;
		} else {
			lateButtonRet = lateButton;
		}
		return false;
	}

	protected _CursorButton raycastIfNull(_CursorButton button){
		if (button == null) {
			return RaycastAButton ();
		} else {
			return button;
		}
	}

	protected void ContinuedHover(){
		if (rect.position != lastPos) {
			lastPos = rect.position;
			hoverButton = RaycastAButton ();
			if (pastHoverButton != hoverButton) {
				if (hoverButton != null) {
					hoverButton.OnHoverEnter (playerID);
				} 
				if (pastHoverButton != null) {
					pastHoverButton.OnHoverExit (playerID);
				}
			} else if (hoverButton != null) {
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
