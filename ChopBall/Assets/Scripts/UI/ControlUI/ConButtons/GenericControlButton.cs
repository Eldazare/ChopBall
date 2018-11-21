using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericControlButton : _ControlButton {

	public UnityEvent ResponseToClick;

	override
	public void OnButtonClick(int playerID){
		ResponseToClick.Invoke ();
	}
}
