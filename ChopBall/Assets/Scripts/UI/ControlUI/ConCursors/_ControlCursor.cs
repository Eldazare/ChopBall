using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ControlCursor : MonoBehaviour {

	// TODO: Saved position on character select
	public MenuPanelHandler menuPanelHandler;
	public int playerID;
	private DPosition currentPosition;
	private _ControlButton currentButton;
	private float treshold = 0.5f;
	private bool inputDone = false;
	private bool triggered = false;

	private bool lateSubmit;
	private bool lateCancel;
	private bool latePaddleLeft;
	private bool latePaddleRight;
	private Vector2 vec;

	private List<DPosition> dirList = new List<DPosition>() {new DPosition(1,0), new DPosition(-1,0), 
		new DPosition(0,1), new DPosition(0,-1)};

	void Start(){
		SetPosition (new DPosition (0, 0));
	}

	public void SetPosition(DPosition newPosition){
		currentButton = menuPanelHandler.GoAnywhere (newPosition, out currentPosition);	
		transform.position = currentButton.transform.position;
	}

	public void GetInput(InputModel model){
		triggered = false;
		for (int i = 0; i < dirList.Count; i++) {
			//Debug.Log ("Magnitude:" + (model.leftDirectionalInput * dirList [i]).magnitude);
			vec = model.leftDirectionalInput * dirList[i];
			if ((vec.x+vec.y) > treshold) {
				Debug.Log (dirList [i].x +"  "+ dirList[i].y);
				triggered = true;
				if (!inputDone) {
					currentButton.OnButtonExit (playerID);
					SetPosition (currentPosition + dirList [i]);
					inputDone = true;
					currentButton.OnButtonEnter (playerID);
					Debug.LogWarning ("Did");
				}
				break;
			}
		}
		if (!triggered) {
			inputDone = false;
		}

		if (UIHelpMethods.IsButtonTrue(model.Submit, lateSubmit, out lateSubmit)) {
			currentButton.OnButtonClick (playerID);
		}
		if (UIHelpMethods.IsButtonTrue(model.Cancel, lateCancel, out lateCancel)) {
			menuPanelHandler.Back ();
		}
		if (UIHelpMethods.IsButtonTrue(model.PaddleLeft, latePaddleLeft, out latePaddleLeft)) {
			currentButton.OnButtonLeftBumper (playerID);
		}
		if (UIHelpMethods.IsButtonTrue(model.PaddleRight, latePaddleRight, out latePaddleRight)) {
			currentButton.OnButtonRightBumper (playerID);
		}
	}
}
