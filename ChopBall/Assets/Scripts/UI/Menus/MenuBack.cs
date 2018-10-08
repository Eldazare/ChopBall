using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBack : MonoBehaviour
{

	public static Previous currentPanel;

    public void Back()
    {
		if (currentPanel.previousPanel != null) {
			currentPanel.gameObject.SetActive (false);
			currentPanel.previousPanel.gameObject.SetActive (true);
		} else {
			Debug.Log ("previousPanel was null. Are we at start panel?");
		}

        /*GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Menu");

        foreach (GameObject go in gameObjectArray)
        {
            go.SetActive(false);
        }*/
    }

	public static void SetCurrentPanel(Previous currentPan){
		currentPanel = currentPan;
		Debug.Log ("Current panel = " + currentPanel.name);
	}
}
