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

	private List<DPosition> dirList = new List<DPosition>() {new DPosition(1,0), new DPosition(-1,0), 
		new DPosition(0,1), new DPosition(0,-1)};

	public void SetPosition(DPosition newPosition){
		currentButton = menuPanelHandler.GoAnywhere (newPosition, out currentPosition);	
	}

	public void GetInput(InputModel model){
		triggered = false;
		for (int i = 0; i < dirList.Count; i++) {
			if ((model.leftDirectionalInput * dirList [i]).magnitude > treshold) {
				triggered = true;
				if (!inputDone) {
					currentButton.OnButtonExit (playerID);
					currentButton = menuPanelHandler.GoAnywhere (currentPosition + dirList [i], out currentPosition); 
					inputDone = true;
					currentButton.OnButtonEnter (playerID);
					break;
				}
			}
		}
		if (!triggered) {
			inputDone = false;
		}

		if (model.Submit) {
			currentButton.OnButtonClick (playerID);
		}
		if (model.Cancel) {
			// TODO: invoke cancel event
		}
		if (model.PaddleLeft) {
			currentButton.OnButtonLeftBumper (playerID);
		}
		if (model.PaddleRight) {
			currentButton.OnButtonRightBumper (playerID);
		}
	}
}
