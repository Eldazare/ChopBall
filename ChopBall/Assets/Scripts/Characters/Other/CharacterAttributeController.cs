using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CharacterAttributeController {

	private static CharacterAttributeData[] characters;

	private static void LoadCharacters(){
		if (characters == null) {
			characters = Resources.LoadAll ("Scriptables/Characters/", typeof(CharacterAttributeData)).Cast<CharacterAttributeData>().ToArray ();
			if (characters == null){
				Debug.LogError("CharacterAttributeData loading failed.");
			}
		}
	}

	public static CharacterAttributeData[] GetCharacters(){
		LoadCharacters ();
		return characters;
	}

	public static CharacterAttributeData GetACharacter(int index){
		LoadCharacters ();
		if (index < 0 || index >= characters.Length) {
			return null;
		}else {
			return characters [index];
		}
	}

	public static string GetCharacterPrefabPreString(){
		return "Prefabs/Characters/";
	}
}
