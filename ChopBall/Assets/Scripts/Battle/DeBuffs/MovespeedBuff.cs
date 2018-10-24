using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovespeedBuff : _Buff {

	public MovespeedBuff(float timeLeft, float magnitude) : base(timeLeft, magnitude){
	
	}

	override
	public void ModifyMods(bool IsActive){
		if (IsActive) {
			mods.movespeedMod *= magnitude;
		} else {
			mods.movespeedMod /= magnitude;
		}
	}
}
