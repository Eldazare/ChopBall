using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ControlCursor : MonoBehaviour {

	// TODO: Saved position on character select
	public MenuPanelHandler menuPanelHandler;
	public GameEvent OnUICancel;
	public int playerID;
	public RectTransform selfRect;
	public float offset;
	private DPosition currentPosition;
	private _ControlButton currentButton;
	private bool inputDone = false;
	private bool triggered = false;

	private bool lateSubmit = false;
	private bool lateCancel = false;
	private bool latePaddleLeft = false;
	private bool latePaddleRight = false;
	private Vector2 vec;
	private DPosition dpos;

	void OnEnable(){
		lateSubmit = true;
		lateCancel = true;
		latePaddleLeft = true;
		latePaddleRight = true;
	}

	public void OnEnableCursor(){
		SetPosition (new DPosition (0, 0));
	}

	public void SetPosition(DPosition newPosition){
		currentButton = menuPanelHandler.GoAnywhere (newPosition, out currentPosition);	
		currentButton.OnButtonEnter (playerID);
		transform.position = currentButton.transform.position;
		SetSizeFromCurrentButton ();
	}

	public void GetInput(InputModel model){
		dpos = UIHelpMethods.CheckDirInput (model, ref triggered, ref inputDone,ref vec);
		if (dpos != null) {
			currentButton.OnButtonExit (playerID);
			SetPosition (currentPosition + dpos);
			currentButton.OnButtonEnter (playerID);
		}
	
		if (UIHelpMethods.IsButtonTrue(model.Submit, ref lateSubmit)) {
			currentButton.OnButtonClick (playerID);
		}
		if (UIHelpMethods.IsButtonTrue(model.Cancel, ref lateCancel)) {
			OnUICancel.Raise ();
		}
		if (UIHelpMethods.IsButtonTrue(model.Dash, ref latePaddleLeft)) {
			currentButton.OnButtonLeftBumper (playerID);
		}
		if (UIHelpMethods.IsButtonTrue(model.Strike, ref latePaddleRight)) {
			currentButton.OnButtonRightBumper (playerID);
		}
	}

	private void SetSizeFromCurrentButton(){
		Rect currentButtonRect = currentButton.GetComponent<RectTransform> ().rect;
		selfRect.sizeDelta = new Vector2 (currentButtonRect.width + offset,
			currentButtonRect.height + offset);
	}
}
