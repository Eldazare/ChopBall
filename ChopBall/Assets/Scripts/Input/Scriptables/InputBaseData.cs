using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class InputBaseData : ScriptableObject {

	public AnimationCurve sensitivityCurve;
	public AnimationCurve smoothingCurve;
	public AnimationCurve velocitySmoothingCurve;
	public float inputSmoothing;
}
