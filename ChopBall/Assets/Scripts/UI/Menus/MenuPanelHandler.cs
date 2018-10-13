using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelHandler : MonoBehaviour
{
	public PanelScript firstPanel;
	public static PanelScript currentPanel;
	public List<ChoiceCursor> playerCursors;
	public MasterCursor masterCursors;

	public void Back()
    {
		if (currentPanel.previousPanel != null) {
			SetCursors (currentPanel.previousPanel.masterZone);
			currentPanel.gameObject.SetActive (false);
			currentPanel.previousPanel.gameObject.SetActive (true);
			currentPanel = currentPanel.previousPanel;
		} else {
			Debug.Log ("previousPanel was null. Are we at start panel?");
		}

        /*GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Menu");

        foreach (GameObject go in gameObjectArray)
        {
            go.SetActive(false);
        }*/
    }

	public void Forward(PanelScript nextPanel){
		SetCursors (nextPanel.masterZone);
		nextPanel.gameObject.SetActive (true);
		if (currentPanel == null) {
			currentPanel = firstPanel;
		}
		currentPanel.gameObject.SetActive (false);
		currentPanel = nextPanel;
	}

	private void SetCursors(bool way){
		masterCursors.gameObject.SetActive (way);
		foreach (ChoiceCursor cursor in playerCursors) {
			cursor.gameObject.SetActive (!way);
		}
	}

	public static void SetCurrentPanel(PanelScript currentPan){
		currentPanel = currentPan;
		Debug.Log ("Current panel = " + currentPanel.name);
	}
}
