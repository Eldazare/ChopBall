using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageChooser : MonoBehaviour {

	public Text stageNameSpace;
	public Text statusText;
	public Image overviewImage;
	public Transform instantiatePosFor3DModel;
	public MenuPanelHandler mph;
	public GameObject loadingIndicator;

	private GameObject currentModel;
	private int currentIndex = 0;
	private StageData[] stages;
	private StageTag currentTag;
	private bool currentChoiceAvailable;

	private bool latePaddleLeft;
	private bool latePaddleRight;
	private bool lateSubmit;
	private bool lateCancel;

	void OnEnable(){
		stages = StageDataController.GetStages ();
		currentTag = StageTagHandler.GetTagsFromCurrentPlayers ();
		GetComponent<InputEventListener> ().Event = mph.gridCursor.GetComponent<InputEventListener> ().Event;
		GetComponent<InputEventListener> ().enabled = true;
		loadingIndicator.SetActive (false);
		latePaddleLeft = false;
		latePaddleRight = false;
		lateSubmit = false;
		lateCancel = false;
		SetDisplay (currentIndex);
	}



	public void GetInput(InputModel model){
		if (UIHelpMethods.IsButtonTrue(model.PaddleLeft, latePaddleLeft, out latePaddleLeft)){
			IncDecCurrentIndex (false);
		}
		if (UIHelpMethods.IsButtonTrue (model.PaddleRight, latePaddleRight, out latePaddleRight)) {
			IncDecCurrentIndex (true);
		}
		if (UIHelpMethods.IsButtonTrue (model.Submit, lateSubmit, out lateSubmit)) {
			SelectStage ();
		}
		if (UIHelpMethods.IsButtonTrue (model.Cancel, lateCancel, out lateCancel)) {
			mph.Back ();
		}
	}

	private void IncDecCurrentIndex(bool incDec){
		currentIndex = UIHelpMethods.CheckIndex (currentIndex, stages.Length, incDec);
		SetDisplay (currentIndex);
	}

	private void SetDisplay(int index){
		StageData stage = stages [index];
		if (currentModel != null) {
			DestroyImmediate (currentModel);
		}
		if (stage.stageMenuModel != null) {
			currentModel = (GameObject)Instantiate (stage.stageMenuModel, instantiatePosFor3DModel);
		}
		stageNameSpace.text = stage.stageMenuName;
		if (stage.stageMenuOverviewImage != null) {
			overviewImage.sprite = stage.stageMenuOverviewImage;
		}
		if (stage.PrimaryTag == currentTag) {
			statusText.text = "Primary stage";
			currentChoiceAvailable = true;
		} else if (stage.SecondaryTags.Contains (currentTag)) {
			statusText.text = "Secondary stage";
			currentChoiceAvailable = true;
		} else {
			statusText.text = "Unavailable";
			currentChoiceAvailable = false;
		}
	}

	private void SelectStage(){
		if (currentChoiceAvailable) {
			loadingIndicator.SetActive (true);
			MasterStateController.WriteStageName (stages [currentIndex].stageMenuName);
			MasterStateController.GoToBattle ();
		} else {
			Debug.LogWarning ("Trying to choose unavailable stage.");
		}
	}
}
