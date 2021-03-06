﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChooser : MonoBehaviour {

	public int playerID;
	private PlayerStateData playerStateData;
	private List<CharacterAttributeData> characterAttributes;
	private PlayerBaseData playerBaseData;
	private int currentChoice;
    private CharRendererStorage charModelRenderer;

    private int mainColor;

	public Image background;
	public GameObject indicatorsPanel;
	public GameObject teamChangeIndicators;
	public Text baseText;
	public Text nameText;
	public Text readyText;
	public GameEvent proceedInput;
	public GameEvent OnUICancel;

	public Transform gameObjectLocation;
	private List<Sprite> allSprites; // Backgrounds


	private GameObject currentCharModel;

	private string baseStr = "Press SELECT to activate!";

	private bool lateStart = false;
	private bool lateSelect = false;
	private bool lateSubmit = false;
	private bool lateLeftPaddle = false;
	private bool lateRightPaddle = false;
	private bool lateCancel = false;

	private DPosition dirPosi;
	private bool dirTriggered = false;
	private bool dirInputDone = false;
	private Vector2 dirVec;

	private string hilightSoundPath;
	private string selectSoundPath;

	private bool initialized = false;

	void OnEnable(){
		Initialize ();
		lateStart = true;
		lateSelect = true;
		lateSubmit = true;
		lateLeftPaddle = true;
		lateRightPaddle = true;
		lateCancel = true;
		dirInputDone = true;
		playerStateData.CheckTeamConstraints ();
		SetColor ();
		teamChangeIndicators.SetActive (playerStateData.active && (playerStateData.team != -1));
		UpdateChosenText ();
	}

	private void Initialize(){
		if (!initialized) {
			initialized = true;
			baseText.text = baseStr;
			nameText.text = "";

			hilightSoundPath = SoundPathController.GetPath ("Hilight");
			selectSoundPath = SoundPathController.GetPath ("Select");
			playerStateData = PlayerStateController.GetAState (playerID);
			characterAttributes = CharacterAttributeController.GetCharacters ();
			playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
			allSprites = playerBaseData.GetBGSprites (playerID);
		}
	}

	public void GetInput(InputModel model){
		if (UIHelpMethods.IsButtonTrue(model.Select, ref lateSelect)){
			if (playerStateData.active) {
				Enabled(false);
			} else {
				Enabled(true);
			}
		}
		if (playerStateData.active) {
			if (UIHelpMethods.IsButtonTrue (model.Submit, ref lateSubmit)) {
				ConfirmChoice ();
			}
			if (!playerStateData.CharacterLocked) {
				dirPosi = UIHelpMethods.CheckDirInput (model, ref dirTriggered, ref dirInputDone, ref dirVec);
				if (!object.ReferenceEquals(dirPosi,null)) {
					if (dirPosi.x == 1) {
						IncDecCurrentChoice (true);
					}
					if (dirPosi.x == -1) {
						IncDecCurrentChoice (false);
					}
                    if (dirPosi.y == 1) {
                        IncDecCurrentColor(true);
                    }
                    if (dirPosi.y == -1) {
                        IncDecCurrentColor(false);
                    }
				}
				if (UIHelpMethods.IsButtonTrue (model.Cancel, ref lateCancel)) {
					Enabled (false);
				}
			} else if (UIHelpMethods.IsButtonTrue (model.Cancel, ref lateCancel)) {
				UnchooseChoice ();
			}
			if (UIHelpMethods.IsButtonTrue (model.Start, ref lateStart)) {
				proceedInput.Raise ();
			}
			if (UIHelpMethods.IsButtonTrue (model.Dash, ref lateLeftPaddle)) {
				IncDecTeam (false);
			}
			if (UIHelpMethods.IsButtonTrue (model.Strike, ref lateRightPaddle)) {
				IncDecTeam (true);
			}
		} else if (UIHelpMethods.IsButtonTrue (model.Submit, ref lateSubmit)) {
			Enabled (true);
		}
		if (UIHelpMethods.IsButtonTrue (model.Cancel, ref lateCancel)) {
			OnUICancel.Raise ();
		}
	}

	private void Enabled(bool active){
		if (currentCharModel != null) {
			currentCharModel.SetActive (active);
		}
		if (active) {
			baseText.text = "";
			LoadCharacterattributes (characterAttributes [currentChoice]);		
		} else {
			baseText.text = baseStr;
			nameText.text = "";
			playerStateData.UnChooseCharacter ();
		}
		teamChangeIndicators.SetActive (active && (playerStateData.team != -1));
		nameText.enabled = active;
		PlayerStateController.SetStateActive (playerID - 1, active);
		UpdateChosenText ();
	}

	private void IncDecCurrentChoice(bool incDec){
		FMODUnity.RuntimeManager.PlayOneShot (hilightSoundPath);
        playerStateData.characterPaletteChoice = 0;
		currentChoice = UIHelpMethods.CheckIndex (currentChoice,  characterAttributes.Count, incDec);
		LoadCharacterattributes (characterAttributes [currentChoice]);
	}

    private void IncDecCurrentColor(bool incDec) {
        FMODUnity.RuntimeManager.PlayOneShot(hilightSoundPath);
        playerStateData.characterPaletteChoice = UIHelpMethods.CheckIndex(playerStateData.characterPaletteChoice, characterAttributes[currentChoice].Palettes.Count, incDec);
        SetPaletteToModel();
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
			if (PlayerStateController.ChooseCharacter (playerID, currentChoice)) {
				FMODUnity.RuntimeManager.PlayOneShot (selectSoundPath);
			}
		}
		UpdateChosenText();
	}

	private void UnchooseChoice(){
		playerStateData.UnChooseCharacter ();
		UpdateChosenText ();
	}

	private void SetCharacter(CharacterAttributeData data){
		if (currentCharModel != null) {
			DestroyImmediate (currentCharModel);
		}
		currentCharModel = (GameObject)Instantiate(data.CharacterMenuModelPrefab, gameObjectLocation);
        charModelRenderer = currentCharModel.GetComponent<CharRendererStorage>();
        SetMainColorToModel();
        SetPaletteToModel();
        
    }

    private void SetPaletteToModel() {
        PaletteContainer palette = characterAttributes[currentChoice].Palettes[playerStateData.characterPaletteChoice];
        charModelRenderer.mainRenderer.materials = CharacterColorSetter.SetColorPalette(charModelRenderer.mainRenderer.materials, palette);
        charModelRenderer.SetMaterialToSkin(palette.skin);
    }

    private void SetMainColorToModel() {
        charModelRenderer.mainRenderer.materials = CharacterColorSetter.SetMainColor(charModelRenderer.mainRenderer.materials, playerBaseData.playerColorMaterials[mainColor]);
        charModelRenderer.SetMainMaterialToTip(playerBaseData.playerColorMaterials[mainColor]);
    }

	private void UpdateChosenText(){
		if (playerStateData.active) {
			if (playerStateData.CharacterLocked) {
				readyText.text = "Ready!";
			} else {
				readyText.text = "Choosing....";
			}
			indicatorsPanel.SetActive (!playerStateData.CharacterLocked);
		} else {
			readyText.text = "";
			indicatorsPanel.SetActive (false);
		}
	}

	private void SetColor(){
		if (playerStateData.team != -1) {
            mainColor = playerStateData.team;
		} else {
            mainColor = playerID - 1;
		}
        background.sprite = allSprites[mainColor];
        if (charModelRenderer != null) {
            SetMainColorToModel();
        }

    }
}
