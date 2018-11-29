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
	public GameEvent OnUICancel;

	private DisplayBaseData dbd;
	private List<GameObject> menuModels;
	private int previousIndex = -1;
	private Vector3 previousIndexScale;
	private int currentIndex = 0;
	private Vector3 currentIndexScale;
	private StageData[] stages;
	private StageTag currentTag;
	private bool currentChoiceAvailable;
	private bool lerpActive;
	private bool lerpBoostActive;
	private float startTime;
	private float startPos;
	private float distanceToLerp;

	private Vector3 smallScaleVector;
	private Vector3 largeScaleVector;

	private bool latePaddleLeft;
	private bool latePaddleRight;
	private bool lateSubmit;
	private bool lateCancel;
	private bool dirTriggered;
	private bool dirInputDone;
	private Vector2 dirVec;
	private DPosition dirPosi;

	private bool initialized = false;

	void Initialize(){
		if (!initialized) {
			dbd = (DisplayBaseData)Resources.Load ("Scriptables/_BaseDatas/DisplayBaseData", typeof(DisplayBaseData));
			smallScaleVector = new Vector3 (dbd.smallScaleMult*dbd.baseScale, dbd.smallScaleMult*dbd.baseScale, dbd.smallScaleMult*dbd.baseScale);
			largeScaleVector = new Vector3 (dbd.largeScaleMult*dbd.baseScale, dbd.largeScaleMult*dbd.baseScale, dbd.largeScaleMult*dbd.baseScale);
			initialized = true;
		}
	}

	void OnEnable(){
		Initialize ();
		stages = StageDataController.GetStages ();
		menuModels = new List<GameObject> (stages.Length);
		for(int i = 0;i < stages.Length;i++) {
			GameObject modelTemp = (GameObject) Instantiate (stages[i].stageMenuModel, instantiatePosFor3DModel);
			modelTemp.transform.localPosition = new Vector3(dbd.xDistBetween * i, 0 ,0);
			modelTemp.transform.localScale = smallScaleVector;
			menuModels.Add (modelTemp);
		}
		menuModels [currentIndex].transform.localScale = largeScaleVector;
		currentTag = StageTagHandler.GetTagsFromCurrentPlayers ();
		GetComponent<InputEventListener> ().Event = mph.gridCursor.GetComponent<InputEventListener> ().Event;
		GetComponent<InputEventListener> ().enabled = true;
		loadingIndicator.SetActive (false);
		latePaddleLeft = true;
		latePaddleRight = true;
		lateSubmit = true;
		lateCancel = true;
		dirTriggered = true;
		dirInputDone = true;
		OnLerpFinish (currentIndex);
	}

	void Update(){
		if (lerpActive) {
			float speed = dbd.lerpSpeedMultiplier * dbd.xDistBetween;
			if (lerpBoostActive) {
				speed *= dbd.lerpListEndBoostMultiplier;
			}
			float frac = (Time.time - startTime) * speed / distanceToLerp;
			float xPos = Mathf.Lerp (startPos, -dbd.xDistBetween * currentIndex, frac);
			instantiatePosFor3DModel.localPosition = new Vector3 (xPos, instantiatePosFor3DModel.localPosition.y, instantiatePosFor3DModel.localPosition.z);
			menuModels [currentIndex].transform.localScale = Vector3.Lerp (currentIndexScale, largeScaleVector, frac);
			menuModels [previousIndex].transform.localScale = Vector3.Lerp (previousIndexScale, smallScaleVector, frac);
			//Debug.Log (frac);
			if (frac > 1) {
				OnLerpFinish (currentIndex);
			}
		} else {
			// Rotate
			menuModels[currentIndex].transform.Rotate(new Vector3(0,dbd.stillRotationSpeedMultiplier*Time.deltaTime,0));
		}
	}



	public void GetInput(InputModel model){
		dirPosi = UIHelpMethods.CheckDirInput (model, ref dirTriggered, ref dirInputDone, ref dirVec);
		if (dirPosi != null) {
			if (dirPosi.x == 1) {
				IncDecCurrentIndex (true);
			}
			if (dirPosi.x == -1) {
				IncDecCurrentIndex (false);
			}
		}
		if (UIHelpMethods.IsButtonTrue(model.Dash, ref latePaddleLeft)){
			IncDecCurrentIndex (false);
		}
		if (UIHelpMethods.IsButtonTrue (model.Strike, ref latePaddleRight)) {
			IncDecCurrentIndex (true);
		}
		if (UIHelpMethods.IsButtonTrue (model.Submit, ref lateSubmit)) {
			SelectStage ();
		}
		if (UIHelpMethods.IsButtonTrue (model.Cancel, ref lateCancel)) {
			OnUICancel.Raise ();
		}
	}

	private void IncDecCurrentIndex(bool incDec){
		previousIndex = currentIndex;
		currentIndex = UIHelpMethods.CheckIndex (currentIndex, stages.Length, incDec);
		StartLerp ();
	}

	private void StartLerp(){
		menuModels [previousIndex].transform.localEulerAngles = Vector3.zero;
		previousIndexScale = menuModels [previousIndex].transform.localScale;
		currentIndexScale = menuModels [currentIndex].transform.localScale;
		currentChoiceAvailable = false;
		startPos = instantiatePosFor3DModel.localPosition.x;
		distanceToLerp = Mathf.Abs (startPos - (-dbd.xDistBetween) * currentIndex);
		if (distanceToLerp > 1.5f * dbd.xDistBetween) {
			lerpBoostActive = true;			
		} else {
			lerpBoostActive = false;
		}
		startTime = Time.time;
		lerpActive = true;
	}

	private void OnLerpFinish(int index){
		lerpActive = false;
		StageData stage = stages [index];
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
