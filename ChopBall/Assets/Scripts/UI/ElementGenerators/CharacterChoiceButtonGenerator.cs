using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoiceButtonGenerator : MonoBehaviour {

	public GameObject characterButtonPanel;
	public PlayerBaseData pBaseData;

	void Awake(){
		pBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData");
		//InitializeColors ();
		InitializeButtonsFromPanel ();
	}

	void OnEnable(){
		PlayerStateController.SetCharacterChoosing (true);
	}

	void OnDisable(){
		PlayerStateController.SetCharacterChoosing(false);
	}

	public void InitializeColors(){
		PlayerActivateTeamButton[] buttons = characterButtonPanel.GetComponentsInChildren<PlayerActivateTeamButton> ();
		if (MasterStateController.GetTheMasterData ().teams) {
			PlayerStateData[] states = PlayerStateController.GetAllStates ();
			for (int i = 0; i < buttons.Length; i++) {
				buttons [i].SetColor (pBaseData.teamColors [states [i].team]);
			}
		} else {
			for(int i = 0;i<buttons.Length;i++) {
				buttons[i].SetColor (pBaseData.playerColors [i]);
			}
		}
	}

	public void InitializeButtonsFromPanel(){
		List<CharacterAttributeData> attributes = CharacterAttributeController.GetCharacters ();
		CharacterChoiceButton[] buttons = characterButtonPanel.GetComponentsInChildren<CharacterChoiceButton> ();
		int indexCap;
		if (attributes.Count > buttons.Length) {
			indexCap = buttons.Length;
		} else {
			indexCap = attributes.Count;
		}
		Debug.Log ("Attributes found: " + attributes.Count + " | Buttons found: " + buttons.Length);
		for (int i = 0; i < indexCap; i++) {
			buttons [i].Initialize (i, attributes[i].CharacterPortraits[0], attributes[i].CharacterName);
		}
		for (int i = indexCap; i<buttons.Length;i++){
			buttons [i].gameObject.SetActive (false);
		}
	}
}
