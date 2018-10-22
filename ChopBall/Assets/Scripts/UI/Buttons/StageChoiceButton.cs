using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageChoiceButton : _CursorButton {

	private StageData stageData;
	protected Image overlay;
	private Outline outline;
	protected bool clickable;

	public void Initialize(StageData stageData){
		if (!awaken) {
			Awaken ();
		}
		overlay = transform.GetChild(0).GetComponent<Image>();
		outline = GetComponent<Outline> ();
		outline.effectColor = baseData.ScbOutlineColor;
		this.stageData = stageData;
		overlay.sprite = stageData.previewImage;
	}

	virtual public void CheckPrimaryTag(StageTag primaryTag){
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
		overlay.color = baseData.ScbMaskHilight;
	}

	private void Normal(){
		clickable = true;
		outline.enabled = false;
		overlay.color = baseData.ScbMaskNeutral;
	}

	private void Darken(){
		clickable = false;
		outline.enabled = false;
		overlay.color = baseData.ScbMaskDarken;
	}

	override
	public void Click(int playerID){
		if (clickable) {
			if (playerID > 0) {
				PlayerStateController.ChooseStage (playerID, stageData.stageName);
			} else if (playerID == 0) {
				MasterStateController.WriteStageName (stageData.stageName);
				MasterStateController.GoToBattle ();
			} else {
				Debug.LogError ("Invalid playerID given :" + playerID);
			}
		}
	}

	override
	public void OnHoverEnter(int playerID){
		if (clickable) {
			base.OnHoverEnter (playerID);
		}
	}

	override
	public void Hover(int playerID){
		Debug.Log ("Hovering over stage choice: " + stageData.stageName);
	}

	override
	public void OnHoverExit(int playerID){
		if (clickable) {
			base.OnHoverExit (playerID);
		}
	}
}
