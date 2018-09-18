using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericCursorButton : _CursorButton {

	public UnityEvent ResponseToClick;

	override
	public void Click(int playerID){
		ResponseToClick.Invoke ();
	}

	override
	public void OnHoverEnter(int playerID){
		
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hover over generic button");
	}

	override
	public void OnHoverExit(int playerID){
		//
	}
}
