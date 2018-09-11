using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStateData : ScriptableObject {

	public bool active = false;
	public bool XYmovementLocked = false;
	public int characterChoice = -1;
	public int stageChoice = -1;
	//TODO, more stuff


	public void GetDefaults(){
		//TODO once above is finished
	}
}
