using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class _CursorButton : MonoBehaviour {

	protected List<int> hoveringPlayers;
	protected Image buttonImage;
	protected CursorButtonBaseData baseData;

	protected virtual void Awake(){
		hoveringPlayers = new List<int> ();
		buttonImage = GetComponent<Image> ();
		baseData = (CursorButtonBaseData) Resources.Load ("Scriptables/_BaseDatas/CursorButtonBaseData", (typeof(CursorButtonBaseData)));
		buttonImage.color = baseData.NeutralHilightColor;
	}

	public virtual void Click(int playerID){
		
	}

	public virtual void OnHoverEnter (int playerID){
		if (EnterAPlayer (playerID)) {
			EnterGeneralHilight ();
		}
	}
	public virtual void Hover(int playerID){
		
	}
	public virtual void OnHoverExit(int playerID){
		if (ExitAPlayer (playerID)) {
			ExitGeneralHilight ();
		}
	}

	// First to enter returns true, others false
	protected bool EnterAPlayer(int playerID){
		if (!hoveringPlayers.Contains (playerID)) {
			hoveringPlayers.Add (playerID);
		}
		if (hoveringPlayers.Count == 1) {
			return true;
		} else {
			return false;
		}
	}


	//Last to exit returns true, others false
	protected bool ExitAPlayer(int playerID){
		if (hoveringPlayers.Contains (playerID)) {
			hoveringPlayers.Remove (playerID);
		}
		if (hoveringPlayers.Count == 0) {
			return true;
		} else {
			return false;
		}
	}

	protected void EnterGeneralHilight(){
		buttonImage.color = baseData.HoverHilightColor;
	}

	protected void ExitGeneralHilight(){
		buttonImage.color = baseData.NeutralHilightColor;
	}
}
