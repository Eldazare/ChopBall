using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _CursorButton : MonoBehaviour {

	protected List<int> hoveringPlayers;

	public abstract void Click(int playerID);
	public abstract void OnHoverEnter (int playerID);
	public abstract void Hover(int playerID);
	public abstract void OnHoverExit(int playerID);

	protected void EnterAPlayer(int playerID){
		if (!hoveringPlayers.Contains (playerID)) {
			hoveringPlayers.Add (playerID);
		}
	}

	protected void ExitAPlayer(int playerID){
		if (hoveringPlayers.Contains (playerID)) {
			hoveringPlayers.Remove (playerID);
		}
	}
}
