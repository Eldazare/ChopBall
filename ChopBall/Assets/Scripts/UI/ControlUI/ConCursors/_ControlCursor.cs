using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ControlCursor : MonoBehaviour {

	// TODO: Saved position on character select
	public MenuPanelHandler menuPanelHandler;
	public GameEvent OnUICancel;
	public int playerID;
	private DPosition currentPosition;
	private _ControlButton currentButton;
	private float treshold = 0.5f;
	private bool inputDone = false;
	private bool triggered = false;

	private bool lateSubmit = false;
	private bool lateCancel = false;
	private bool latePaddleLeft = false;
	private bool latePaddleRight = false;
	private Vector2 vec;

	private List<DPosition> dirList = new List<DPosition>() {new DPosition(1,0), new DPosition(-1,0), 
		new DPosition(0,1), new DPosition(0,-1)};

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
	}

	public void GetInput(InputModel model){
		DPosition dpos = UIHelpMethods.CheckDirInput (model, ref triggered, ref inputDone,ref vec);
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
}
