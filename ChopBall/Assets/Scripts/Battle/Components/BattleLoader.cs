using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour {

	//TODO: Reads StateData and initializes components, then invokes startGame
	//TODO: Initialize characters from StateData (and prefabs)
	//TODO: Initialize ball start positions from scene
	//TODO: Initialize goal IDs
	public GameEvent StartGame;

	void Start () {
		CurrentBattleController.InitializeCurrentData ();
		// TODO: Add timer? And timer displayer? (Before start)
		StartGame.Raise();
	}
}
