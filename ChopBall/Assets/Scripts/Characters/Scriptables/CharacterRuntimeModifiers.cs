using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterRuntimeModifiers : ScriptableObject {

	// Preliminary
	public float movespeedMod = 1;



	public void SetDefaults(){
		movespeedMod = 1;
	}
}
