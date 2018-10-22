using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStageButton : StageChoiceButton {


	public Image grLayer2;

	public delegate void StageToggle(int integer);
	private StageToggle toggleMethod;
	private int index;


	override
	public void CheckPrimaryTag(StageTag primaryTag){
		base.CheckPrimaryTag (primaryTag);
		if (clickable) {
			Toggled (false);
		} else {
			grLayer2.color = new Color32 (0, 0, 0, 0);
		}
	}

	public void SetDelegate(StageToggle toggle, int index){
		toggleMethod = toggle;
		this.index = index;
	}

	public void Toggled(bool toggle){
		if (toggle) {
			Green ();
		} else {
			Red ();
		}
	}

	private void Green(){
		grLayer2.color = new Color32 (0, 255, 0, 120);
	}

	private void Red(){
		grLayer2.color = new Color32 (255, 0, 0, 120);
	}

	override
	public void Click(int playerID){
		if (clickable) {
			toggleMethod (index);
		}
	}
}
