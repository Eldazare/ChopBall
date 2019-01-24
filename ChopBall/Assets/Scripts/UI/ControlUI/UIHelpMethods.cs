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

	public static bool IsButtonTrue(bool button, ref bool late){
		if (button && !late) {
			late = true;
			return true;
//		} else if (!button && late) {
//			late = false;
		} else {
			late = button;
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
			vec = model.leftDirectionalInput * dirList[i];
			if ((vec.x-vec.y) > treshold) {
				triggered = true;
				if (!inputDone) {
					inputDone = true;
					return dirList [i];
				}
				break;
			}
			vec = model.D_PadVector * dirList [i];
			if ((vec.x - vec.y) > 0.8f) {
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
		return null;
	}

	public static DPosition GetStickDirection(Vector2 axDir, float treshold, ref bool lateOut){
		// TODO
		return null;
	}
}
