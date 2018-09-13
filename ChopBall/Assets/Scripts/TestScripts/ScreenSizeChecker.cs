using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeChecker : MonoBehaviour {

	public GameEvent OnScreenSizeChange;
	float ScreenX;
	float ScreenY;

	void Start () {
		ScreenX = Screen.width;
		ScreenY = Screen.height;
	}

	void Update () {
		if (ScreenX != Screen.width && ScreenY != Screen.height) {
			ScreenX = Screen.width;
			ScreenY = Screen.height;
			OnScreenSizeChange.Raise ();
			Debug.Log ("screen size changed");
		}
	}
}
