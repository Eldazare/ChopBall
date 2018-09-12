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


	public void SetDefaultValues(){
		active = false;
		XYmovementLocked = false;
		characterChoice = -1;
		stageChoice = -1;
		// TODO add as they come
	}
}
