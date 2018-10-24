using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Buff {

	public float timeLeft;
	public float magnitude;
	public CharacterRuntimeModifiers mods;

	private bool eternal;

	public _Buff (float timeLeft, float magnitude){
		this.timeLeft = timeLeft;
		eternal = false;
		if (timeLeft == 0) {
			eternal = true;
		}
		this.magnitude = magnitude;
	}

	public void GiveMods(CharacterRuntimeModifiers mods){
		this.mods = mods;
	}

	virtual
	public bool ReduceTime(float deltaTime){
		if (!eternal) {
			timeLeft -= deltaTime;
			if (timeLeft < 0) {
				Die ();
				return true;
			}
		}
		return false;
	}

	virtual public void Die(){
		ModifyMods (false);
	}

	abstract
	public void ModifyMods (bool IsActivate);
}
