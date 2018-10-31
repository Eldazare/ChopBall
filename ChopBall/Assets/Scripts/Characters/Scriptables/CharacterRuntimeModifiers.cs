using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterRuntimeModifiers : ScriptableObject {

	[Header("NonMultipliers")]
	public float stamina = 0;

	[Header("Multipliers")]
	public float movespeedMod = 1;
	public float staminaRegen = 1;



	public void SetDefaults(){
		stamina = 100;
		movespeedMod = 1;
		staminaRegen = 1;
	}
}
