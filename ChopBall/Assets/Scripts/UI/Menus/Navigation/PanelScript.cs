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

	void Awake(){
		AssignToListList (GetComponentsInChildren<_ControlButton> ());
	}

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
}
