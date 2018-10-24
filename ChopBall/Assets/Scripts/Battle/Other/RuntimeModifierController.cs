using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RuntimeModifierController {

	public static CharacterRuntimeModifiers[] modControllers;

	private static void LoadMods(){
		if (modControllers == null) {
			modControllers = Resources.LoadAll ("Scriptables/Players/CharacterRuntimeMods/", typeof(CharacterRuntimeModifiers))
				.Cast<CharacterRuntimeModifiers> ().ToArray ();
			foreach (var mod in modControllers) {
				mod.SetDefaults ();
			}
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
}
