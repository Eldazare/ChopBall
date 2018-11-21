using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RuntimeModifierController {

	private static CharacterRuntimeModifiers[] modControllers;
	private static List<CharacterAttributeData> charAttributeList;
	private static CharacterBaseData charBaseData;

	private static float deltaTimeBase;

	private static void LoadMods(){
		if (modControllers == null) {
			modControllers = Resources.LoadAll ("Scriptables/Players/CharacterRuntimeMods/", typeof(CharacterRuntimeModifiers))
				.Cast<CharacterRuntimeModifiers> ().ToArray ();
			foreach (var mod in modControllers) {
				mod.SetDefaults ();
			}
			charAttributeList = new List<CharacterAttributeData> (4);
			charBaseData = (CharacterBaseData)Resources.Load ("Scriptables/_BaseDatas/CharacterBaseData");
		}
	}

	public static CharacterRuntimeModifiers[] GetAllMods(){
		LoadMods ();
		return modControllers;
	}

	public static CharacterRuntimeModifiers GetAMod(int playerID){
		LoadMods ();
		return modControllers [playerID];
	}

	public static CharacterRuntimeModifiers GetGlobalMod(){
		LoadMods ();
		return modControllers [0];
	}

	public static void ClearAttributeDatas(){
		LoadMods ();
		charAttributeList.Clear ();
		for (int i = 0; i<modControllers.Length-1;i++){
			charAttributeList.Add (null);
		}
	}

	public static void AddAttributeData(CharacterAttributeData attributeData, int playerIndex){
		LoadMods ();
		charAttributeList[playerIndex] = (attributeData);
	}

    // No effect from attribute data
	public static void ProgressTime(float deltaTime){
		LoadMods ();
		deltaTimeBase = deltaTime * charBaseData.StaminaRegen;
		for (int i = 1; i < modControllers.Length; i++) {
			modControllers [i].stamina += deltaTimeBase * modControllers[i].staminaRegen;
			//Debug.Log (deltaTimeBase * charAttributeList [i - 1].StaminaRegen * modControllers [i].staminaRegen);
			if (modControllers [i].stamina > charBaseData.StaminaMax) {
				modControllers [i].stamina = charBaseData.StaminaMax;
			}
		}
	}
}
