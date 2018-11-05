using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelScript : MonoBehaviour
{
	public PanelScript previousPanel;
	public bool masterZone;

	public List<List<_ControlButton>> buttonList;

	public UnityEvent OnPanelEnter;

	/*
	void OnEnable(){
		MenuPanelHandler.SetCurrentPanel (this);
	}
	*/

}
