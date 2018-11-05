using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CursorActiveStatus{None, PlayerCursors, MasterCursor}

public class PanelScript : MonoBehaviour
{
	public PanelScript previousPanel;
	public bool masterZone;
	public CursorActiveStatus cursorActiveStatus; // TODO: Implement instead of masterZone

	public List<List<PanelScript>> subPanels;

	public List<List<_ControlButton>> buttonList;

	public UnityEvent OnPanelEnter;

	/*
	void OnEnable(){
		MenuPanelHandler.SetCurrentPanel (this);
	}
	*/

}
