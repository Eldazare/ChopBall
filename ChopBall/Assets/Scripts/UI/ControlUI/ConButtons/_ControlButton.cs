using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _ControlButton : MonoBehaviour {

	//public DPosition dpos;

	virtual
	public void OnButtonEnter(int playerID){
		
	}

	virtual
	public void OnButtonExit(int playerID){
		
	}

	virtual
	public void OnButtonClick(int playerID){
	
	}

	virtual 
	public void OnButtonLeftBumper(int playerID){
		
	}

	virtual 
	public void OnButtonRightBumper(int playerID){
	
	}
}
