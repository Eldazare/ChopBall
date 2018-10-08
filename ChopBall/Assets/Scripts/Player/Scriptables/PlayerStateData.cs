using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStateData : ScriptableObject {

	public bool active = false;
	public bool XYmovementLocked = false;
	public int characterChoice = -1;
	public int team = -1; // From 0 to 3
	public string stageNameChoice = "";
	//TODO, more stuff


	public void SetDefaultValues(){
		active = false;
		XYmovementLocked = false;
		characterChoice = -1;
		team = -1;
		stageNameChoice = "";
		// TODO add as they come
	}
}
