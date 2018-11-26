using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelpMethods {

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

	public static DPosition GetStickDirection(Vector2 axDir, float treshold, ref bool lateOut){
		// TODO
		return null;
	}
}
