using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PanelScript : MonoBehaviour
{
	public PanelScript previousPanel;
	public PanelScript nextPanel;
	public bool gridMenuZone;
	public bool helpSubmenuActive;

	[Header("Rows")]
	public List<_ControlButton> row0;
	public List<_ControlButton> row1;
	public List<_ControlButton> row2;
	public List<_ControlButton> row3;
	public List<_ControlButton> row4;

	public List<List<PanelScript>> subPanels;

	public List<List<_ControlButton>> buttonList;

	public UnityEvent OnPanelEnter;

	public DPosition lastPosition;

	void Awake(){
		buttonList = new List<List<_ControlButton>> ();
		AddIfNotEmpty (buttonList, row0);
		AddIfNotEmpty (buttonList, row1);
		AddIfNotEmpty (buttonList, row2);
		AddIfNotEmpty (buttonList, row3);
		AddIfNotEmpty (buttonList, row4);
		//AssignToListList (GetComponentsInChildren<_ControlButton> ());
	}

	private List<List<_ControlButton>> AddIfNotEmpty(List<List<_ControlButton>> masterList, List<_ControlButton> list){
		if (list.Count > 0) {
			masterList.Add (list);
		}
		return masterList;
	}

	/*
	private void AssignToListList(_ControlButton[] arr){
		buttonList = new List<List<_ControlButton>> ();
		foreach (var bu in arr) {
			AssignSingle (bu);
		}
		foreach (var l in buttonList) {
			Debug.Log ("C: "+l.Count);
		}
	}

	private void AssignSingle(_ControlButton button){
		while (buttonList.Count <= button.dpos.y) {
			buttonList.Add (new List<_ControlButton> ());
		}
		while (buttonList [button.dpos.y].Count <= button.dpos.x) {
			buttonList[button.dpos.y].Add (null);
		}
		buttonList [button.dpos.y] [button.dpos.x] = button;
	}
	*/
}
