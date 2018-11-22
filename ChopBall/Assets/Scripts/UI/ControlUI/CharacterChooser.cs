using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChooser : MonoBehaviour {

	public int playerID;
	private PlayerStateData playerStateData;
	private List<CharacterAttributeData> characterAttributes;
	private int currentChoice;

	public Text baseText;
	public Text nameText;
	public Text readyText;
	public GameEvent proceedInput;

	public Transform gameObjectLocation;
	private GameObject currentCharModel;

	private string baseStr = "Press SELECT to activate!";

	private bool lateStart = false;
	private bool lateSelect = false;
	private bool lateSubmit = false;
	private bool lateLeftPaddle = false;
	private bool lateRightPaddle = false;

	void Awake(){
		baseText.text = baseStr;
		nameText.text = "";
	}

	void OnEnable(){
		playerStateData = PlayerStateController.GetAState (playerID);
		characterAttributes = CharacterAttributeController.GetCharacters ();
		UpdateChosenText ();
		lateStart = true;
		lateSelect = true;
		lateSubmit = true;
		lateLeftPaddle = true;
		lateRightPaddle = true;
	}

	public void GetInput(InputModel model){
		if (UIHelpMethods.IsButtonTrue(model.Select, lateSelect, out lateSelect)){
			if (playerStateData.active) {
				Enabled(false);
				baseText.text = baseStr;
				nameText.text = "";
				playerStateData.CharacterLocked = false;
			} else {
				Enabled(true);
				baseText.text = "";
				LoadCharacterattributes (characterAttributes [currentChoice]);
			}
			UpdateChosenText ();
		}
		if (playerStateData.active) {
			if (UIHelpMethods.IsButtonTrue (model.Submit, lateSubmit, out lateSubmit)) {
				ConfirmChoice ();
			}
			if (!playerStateData.CharacterLocked) {
				if (UIHelpMethods.IsButtonTrue (model.PaddleLeft, lateLeftPaddle, out lateLeftPaddle)) {
					IncDecCurrentChoice (false);
				}
				if (UIHelpMethods.IsButtonTrue (model.PaddleRight, lateRightPaddle, out lateRightPaddle)) {
					IncDecCurrentChoice (true);
				}
			} else if (model.Cancel) {
				ConfirmChoice ();
			}
			if (UIHelpMethods.IsButtonTrue(model.Start, lateStart, out lateStart)){
				proceedInput.Raise();
			}
		}
	}

	private void Enabled(bool active){
		if (currentCharModel != null) {
			currentCharModel.SetActive (active);
		}
		nameText.enabled = active;
		PlayerStateController.SetStateActive (playerID - 1, active);
	}

	private void IncDecCurrentChoice(bool incDec){
		currentChoice = UIHelpMethods.CheckIndex (currentChoice,  characterAttributes.Count, incDec);
		LoadCharacterattributes (characterAttributes [currentChoice]);
	}

	private void IncDecTeam(bool incDec){
		playerStateData.ChangeTeam (incDec);
	}

	private void LoadCharacterattributes(CharacterAttributeData data){
		SetCharacter (data);
		nameText.text = data.CharacterName;
	}

	private void ConfirmChoice(){
		if (playerStateData.active) {
			PlayerStateController.ChooseCharacter (playerID, currentChoice);
		}
		UpdateChosenText();
	}

	private void SetCharacter(CharacterAttributeData data){
		if (currentCharModel != null) {
			DestroyImmediate (currentCharModel);
		}
		currentCharModel = (GameObject)Instantiate(data.CharacterMenuModelPrefab, gameObjectLocation);
	}

	private void UpdateChosenText(){
		if (playerStateData.active) {
			if (playerStateData.CharacterLocked) {
				readyText.text = "Ready!";
			} else {
				readyText.text = "Choosing....";
			}
		} else {
			readyText.text = "";
		}
	}
}
