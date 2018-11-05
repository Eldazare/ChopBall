﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelHandler : MonoBehaviour
{
	// TODO: ControlCursor positionSet event
	public PanelScript firstPanel;
	public static PanelScript currentPanel;
	public List<ChoiceCursor> playerCursors;
	public MasterCursor masterCursors;

	void Awake(){
		currentPanel = firstPanel;
	}

	public void Back()
    {
		if (currentPanel.previousPanel != null) {
			currentPanel.gameObject.SetActive (false);
			currentPanel.previousPanel.gameObject.SetActive (true);
			currentPanel = currentPanel.previousPanel;
			SetCursors (currentPanel.masterZone);
			currentPanel.OnPanelEnter.Invoke ();
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
		if (currentPanel == null) {
			currentPanel = firstPanel;
		}
		currentPanel.gameObject.SetActive (false);
		currentPanel = nextPanel;
		currentPanel.gameObject.SetActive (true);
		currentPanel.OnPanelEnter.Invoke ();
	}


	public void Close(int tier, int index){
		if (currentPanel.subPanels.Count > tier) {
			if (currentPanel.subPanels [tier].Count > index) {
				currentPanel.subPanels [tier] [index].gameObject.SetActive (false);
				return;
			}
		}
		Debug.LogError ("False Close panel call with: " + tier + " & " + index);
	}

	public void Open(int tier, int index){
		if (currentPanel.subPanels.Count > tier) {
			if (currentPanel.subPanels [tier].Count > index) {
				currentPanel.subPanels [tier] [index].gameObject.SetActive (true);
				return;
			}
		}
		Debug.LogError ("False Open panel call with: " + tier + " & " + index);
	}

	public void SetCursorsActiveFromCurrentAndStates(){
		if (!currentPanel.masterZone) {
			foreach (ChoiceCursor cursor in playerCursors) {
				cursor.CheckActiveState ();
			}
		}
	}

	public void SetCursors(bool isMaster){
		masterCursors.gameObject.SetActive (isMaster);
		foreach (ChoiceCursor cursor in playerCursors) {
			cursor.gameObject.SetActive (!isMaster);
			if (!isMaster) {
				cursor.CheckActiveState ();
			}
		}
	}


	// CONTROL

	public _ControlButton GoAnywhere(DPosition currentPos, out DPosition pos){
		int x = currentPos.x; int y = currentPos.y;
		if (currentPanel.buttonList[currentPos.y].Count <= currentPos.x) {
			x = 0;
		} else if (currentPos.x < 0) {
			x = currentPanel.buttonList[currentPos.y].Count-1;
		}
		if (currentPanel.buttonList.Count <= currentPos.y) {
			y = 0;
		} else if (currentPos.y < 0) {
			y = currentPanel.buttonList.Count - 1;
		}
		if (currentPanel.buttonList [y].Count <= currentPos.x) {
			x = currentPanel.buttonList [y].Count;
		}
		pos = new DPosition (x, y);
		return currentPanel.buttonList [y] [x];
	}

	public void SetCharactersPerPlayerChoice(){
		//??
	}

	public static void SetCurrentPanel(PanelScript currentPan){
		currentPanel = currentPan;
		Debug.Log ("Current panel = " + currentPanel.name);
	}
}


public class DPosition{
	public int x;
	public int y;

	public DPosition(int x, int y){
		this.x = x;
		this.y = y;
	}

	public static Vector2 operator*(Vector2 vec, DPosition dpos){
		vec.x *= dpos.x;
		vec.y *= dpos.y;
		return vec;
	}

	public static DPosition operator+(DPosition dpos1, DPosition dpos2){
		dpos1.x += dpos2.x;
		dpos1.y += dpos2.y;
		return dpos1;
	}
}
