using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour {

	//TODO: Reads StateData and initializes components, then invokes startGame
	public GameEvent StartGame;

	void Awake () {

		//...
		StartGame.Raise();
	}
}
