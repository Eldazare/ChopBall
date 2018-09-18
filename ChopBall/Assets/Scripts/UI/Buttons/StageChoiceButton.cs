using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageChoiceButton : _CursorButton {

	private StageData stageData;
	private Image overlay;
	private Outline outline;
	private bool clickable;

	public void Initialize(StageData stageData){
		overlay = transform.GetChild(0).GetComponent<Image>();
		outline = GetComponent<Outline> ();
		this.stageData = stageData;
		overlay.sprite = stageData.previewImage;
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
		overlay.color = new Color32 (255, 255, 255,0);
	}

	private void Normal(){
		clickable = true;
		outline.enabled = false;
		overlay.color = new Color32 (220,220, 220,100);
	}

	private void Darken(){
		clickable = false;
		outline.enabled = false;
		overlay.color = new Color32 (80, 80, 80,100);
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
	public void OnHoverEnter(int playerID){
		Debug.Log ("Entered clickable: " + clickable);
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hovering over stage choice: " + stageData.stageName);
	}

	override
	public void OnHoverExit(int playerID){
	
	}
}
