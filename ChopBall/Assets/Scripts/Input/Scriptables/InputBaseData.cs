using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class InputBaseData : ScriptableObject {

	public AnimationCurve sensitivityCurve;
	public AnimationCurve smoothingCurve;
	public AnimationCurve velocitySmoothingCurve;
	public float inputSmoothing;
	public float gravityModifier;

	[Range(0.1f,0.99f)]
	public float triggerTreshold = 0.5f;
}
