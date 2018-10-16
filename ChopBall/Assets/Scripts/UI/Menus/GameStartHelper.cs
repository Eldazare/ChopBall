using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartHelper : MonoBehaviour {

	public void StartGame(){
		MasterStateController.GetTheMasterData ().GoToBattle ();
	}
}
