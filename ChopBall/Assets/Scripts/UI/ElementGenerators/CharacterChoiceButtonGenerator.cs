using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoiceButtonGenerator : MonoBehaviour {

	public GameObject characterButtonPanel;

	void Awake(){
		InitializeButtonsFromPanel ();
	}

	void OnEnable(){
		PlayerStateController.SetCharacterChoosing (true);
	}

	void OnDisable(){
		PlayerStateController.SetCharacterChoosing(false);
	}

	public void InitializeButtonsFromPanel(){
		CharacterAttributeData[] attributes = CharacterAttributeController.GetCharacters ();
		CharacterChoiceButton[] buttons = characterButtonPanel.GetComponentsInChildren<CharacterChoiceButton> ();
		int indexCap;
		if (attributes.Length > buttons.Length) {
			indexCap = buttons.Length;
		} else {
			indexCap = attributes.Length;
		}
		Debug.Log ("Attributes found: " + attributes.Length + " | Buttons found: " + buttons.Length);
		for (int i = 0; i < indexCap; i++) {
			buttons [i].Initialize (i, attributes[i].CharacterPortrait, attributes[i].CharacterName);
		}
		for (int i = indexCap; i<buttons.Length;i++){
			buttons [i].gameObject.SetActive (false);
		}
	}
}
