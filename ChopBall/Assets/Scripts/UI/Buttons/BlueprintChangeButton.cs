using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintChangeButton : _CursorButton {

	public delegate void BlueprintIncrement(bool incrementOrDecrement);
	private BlueprintIncrement blueprintIncrement;
	//private Text theText;

	public void Initialize(BlueprintIncrement blueprintInc){
		this.blueprintIncrement = blueprintInc;
	}

	override
	public void Click(int playerID){
		blueprintIncrement.Invoke (true);
	}

	override
	public void OnClickRight(){
		blueprintIncrement.Invoke (true);
	}

	override
	public void OnClickLeft(){
		blueprintIncrement.Invoke (false);
	}

	public void SetString(string str){
		gameObject.GetComponentInChildren<Text> ().text = str;
	}
}
