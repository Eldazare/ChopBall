using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageChoiceButton : _CursorButton {

	private StageData stageData;
	private Image display;
	private Outline outline;
	private bool clickable;

	public void Initialize(StageData stageData){
		display = GetComponent<Image> ();
		outline = GetComponent<Outline> ();
		this.stageData = stageData;
		display.sprite = stageData.previewImage;
	}

	public void CheckPrimaryTag(StageTag primaryTag){
		if (primaryTag == stageData.PrimaryTag) {
			Hilight ();
		} else if (stageData.SecondaryTags.Contains (primaryTag)) {
			Normal ();
		} else {
			Darken ();
		}
	}

	private void Hilight(){
		clickable = true;
		outline.enabled = true;
		display.color = new Color (255, 255, 255);
	}

	private void Normal(){
		clickable = true;
		outline.enabled = false;
		display.color = new Color (255,255, 255);
	}

	private void Darken(){
		clickable = false;
		outline.enabled = false;
		display.color = new Color (100, 100, 100);
	}

	override
	public void Click(int playerID){
		if (clickable) {
			if (playerID > 0) {
				PlayerStateController.ChooseStage (playerID, stageData.stageName);
			} else if (playerID == 0) {
				MasterStateController.WriteStageName (stageData.stageName);
			} else {
				Debug.LogError ("Invalid playerID given :" + playerID);
			}
		}
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hovering over stage choice: " + stageData.stageName);
	}

	override
	public void OnHoverExit(int playerID){
	
	}
}
