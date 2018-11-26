using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChooser : MonoBehaviour {

	public int playerID;
	private PlayerStateData playerStateData;
	private List<CharacterAttributeData> characterAttributes;
	private PlayerBaseData playerBaseData;
	private int currentChoice;

	public Image background;
	public Text baseText;
	public Text nameText;
	public Text readyText;
	public GameEvent proceedInput;
	public GameEvent OnUICancel;

	public Transform gameObjectLocation;
	private GameObject currentCharModel;

	private string baseStr = "Press SELECT to activate!";

	private bool lateStart = false;
	private bool lateSelect = false;
	private bool lateSubmit = false;
	private bool lateLeftPaddle = false;
	private bool lateRightPaddle = false;
	private bool lateCancel = false;
	private bool lateXDir = false;

	void Awake(){
		baseText.text = baseStr;
		nameText.text = "";
	}

	void OnEnable(){
		playerStateData = PlayerStateController.GetAState (playerID);
		characterAttributes = CharacterAttributeController.GetCharacters ();
		playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
		UpdateChosenText ();
		lateStart = true;
		lateSelect = true;
		lateSubmit = true;
		lateLeftPaddle = true;
		lateRightPaddle = true;
		lateCancel = true;
		lateXDir = true;
		playerStateData.CheckTeamConstraints ();
		SetColor ();
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
				if (UIHelpMethods.IsButtonTrue (model.Cancel, lateCancel, out lateCancel)) {
					Enabled (false);
					baseText.text = baseStr;
					nameText.text = "";
					UpdateChosenText ();
				}
			} else if (UIHelpMethods.IsButtonTrue(model.Cancel, lateCancel, out lateCancel)) {
				ConfirmChoice ();
			}
			if (UIHelpMethods.IsButtonTrue(model.Start, lateStart, out lateStart)){
				proceedInput.Raise();
			}
			int stickDir = UIHelpMethods.IsAxisOverTreshold (model.leftDirectionalInput.x, 0.5f, ref lateXDir);
			if (stickDir == 1) {
				IncDecTeam (true);
			}
			if (stickDir == -1) {
				IncDecTeam (false);
			}
		}
		if (UIHelpMethods.IsButtonTrue (model.Cancel, lateCancel, out lateCancel)) {
			OnUICancel.Raise ();
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
		SetColor ();
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

	private void SetColor(){
		if (playerStateData.team != -1) {
			ApplyColor (playerBaseData.teamColors [playerStateData.team]);
		} else {
			ApplyColor (playerBaseData.playerColors [playerID-1]);
		}
	}

	private void ApplyColor(Color32 color){
		background.color = color;
	}
}
