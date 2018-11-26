using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelpMethods {

	private static float treshold = 0.5f;
	private static List<DPosition> dirList = new List<DPosition>() {new DPosition(1,0), new DPosition(-1,0), 
		new DPosition(0,1), new DPosition(0,-1)};

	public static int CheckIndex(int currentIndex, int length, bool incDec){
		if (incDec) {
			currentIndex += 1;
		} else {
			currentIndex -= 1;
		}
		if (currentIndex >= length) {
			return 0;
		} else if (currentIndex < 0) {
			return length - 1;
		}
		return currentIndex;
	}

	public static bool IsButtonTrue(bool button, bool late, out bool lateOut){
		if (button && !late) {
			lateOut = true;
			return true;
		} else if (!button && late) {
			lateOut = false;
		} else {
			lateOut = late;
		}
		return false;
	}

	public static int IsAxisOverTreshold(float axis, float treshold ,ref bool lateOut){
		bool input = false;
		if (axis < -treshold) {
			input = true;
			if (!lateOut) {
				lateOut = true;
				return -1;
			}
		} 
		if (axis > treshold) {
			input = true;
			if (!lateOut) {
				lateOut = true;
				return 1;
			}
		}
		lateOut = input;
		return 0;
	}

	public static DPosition CheckDirInput(InputModel model, ref bool triggered, ref bool inputDone, ref Vector2 vec){
		triggered = false;
		for (int i = 0; i < dirList.Count; i++) {
			//Debug.Log ("Magnitude:" + (model.leftDirectionalInput * dirList [i]).magnitude);
			vec = model.leftDirectionalInput * dirList[i];
			if ((vec.x-vec.y) > treshold) {
				//Debug.Log (dirList [i].x +"  "+ dirList[i].y);
				triggered = true;
				if (!inputDone) {
					inputDone = true;
					return dirList [i];
				}
				break;
			}
		}
		if (!triggered) {
			inputDone = false;
		}
		if (model.D_PadUp) {
			return dirList [3];
		}
		if (model.D_PadDown) {
			return dirList [2];
		}
		if (model.D_PadLeft) {
			return dirList [1];
		}
		if (model.D_PadLeft) {
			return dirList [0];
		}
		return null;
	}

	public static DPosition GetStickDirection(Vector2 axDir, float treshold, ref bool lateOut){
		// TODO
		return null;
	}
}
