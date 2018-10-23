using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CharacterAttributeController {

	private static CharacterAttributeData defaultChar;
	private static List<CharacterAttributeData> characters;

	private static void LoadCharacters(){
		if (characters == null) {
			characters = Resources.LoadAll ("Scriptables/Characters/", typeof(CharacterAttributeData)).Cast<CharacterAttributeData>().ToList();
			defaultChar = characters [0];
			characters.RemoveAt (0);
			if (characters == null){
				Debug.LogError("CharacterAttributeData loading failed.");
			}
		}
	}

	public static List<CharacterAttributeData> GetCharacters(){
		LoadCharacters ();
		return characters;
	}

	public static CharacterAttributeData GetDefaultChar(){
		LoadCharacters ();
		return defaultChar;
	}

	public static CharacterAttributeData GetACharacter(int index){
		LoadCharacters ();
		if (index < 0 || index >= characters.Count) {
			return null;
		}else {
			return characters [index];
		}
	}

	public static string GetCharacterPrefabPreString(){
		return "Prefabs/Characters/";
	}
}
