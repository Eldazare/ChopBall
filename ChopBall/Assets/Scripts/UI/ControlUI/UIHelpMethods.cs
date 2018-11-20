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
}
