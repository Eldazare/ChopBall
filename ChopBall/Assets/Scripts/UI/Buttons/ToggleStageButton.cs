using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStageButton : StageChoiceButton {


	public delegate void StageToggle(int integer);
	private StageToggle toggleMethod;
	private int index;

	public void SetDelegate(StageToggle toggle, int index){
		toggleMethod = toggle;
		this.index = index;
	}

	override
	public void Click(int playerID){
		if (clickable) {
			toggleMethod (index);
		}
	}
}
