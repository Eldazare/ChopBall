using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StageTag {T1v1, T2v2, FFA3, FFA4};

[CreateAssetMenu]
public class StageData : ScriptableObject {

	public string stageName;
	public string stageSceneName;
	public Sprite previewImage;

	public StageTag PrimaryTag;
	public List<StageTag> SecondaryTags;
}
