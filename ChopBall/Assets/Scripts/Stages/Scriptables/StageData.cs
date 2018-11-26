using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StageTag {None, T1v1, T2v2, FFA3, FFA4};

[CreateAssetMenu]
public class StageData : ScriptableObject {

	public string stageMenuName;
	public GameObject stageMenuModel;
	public Sprite stageMenuOverviewImage;

	public string stageSceneName; // Used to load the scene

	public StageTag PrimaryTag;
	public List<StageTag> SecondaryTags;
}
