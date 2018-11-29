using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DisplayBaseData : ScriptableObject {

	// TODO: Use these
	[Header("In-game UI")]
	public float inGameYMultiplier;
	public float inGameXOverheadMultiplier;

	[Header("Stage chooser")]
	[Range(1f,10000f)]
	public float xDistBetween = 500;
	[Range(0.1f,50f)]
	public float lerpSpeedMultiplier = 10;
	[Range(1f,5f)]
	public float lerpListEndBoostMultiplier = 2;
	public float baseScale = 100f;
	[Range(0.1f,1.0f)]
	public float smallScaleMult = 0.4f;
	[Range(1.0f, 2.0f)]
	public float largeScaleMult = 1.0f;
}
