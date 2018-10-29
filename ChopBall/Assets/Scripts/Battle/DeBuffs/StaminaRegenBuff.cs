using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRegenBuff : _Buff {

	public StaminaRegenBuff(float timeLeft, float magnitude) : base(timeLeft, magnitude){

	}

	override
	public void ModifyMods(bool IsActive){
		if (IsActive) {
			mods.staminaRegen *= magnitude;
		} else {
			mods.staminaRegen /= magnitude;
		}
	}
}
