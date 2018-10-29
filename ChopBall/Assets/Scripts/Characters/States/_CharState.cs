using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState{None, Default, Charge, Block}

public abstract class _CharState {

	protected CharacterAttributeData attributeData;
	protected CharacterRuntimeModifiers runtimeMods;
	protected CharacterBaseData baseData;

	public void SetDatas(CharacterAttributeData data1, CharacterRuntimeModifiers data2, CharacterBaseData data3){
		attributeData = data1;
		runtimeMods = data2;
		baseData = data3;
	}

	public abstract CharacterState Input(InputModel model);

	public abstract void OnStateEnter ();

	public abstract void OnStateExit ();

}
