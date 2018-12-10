using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SystemSettings : ScriptableObject {


	[Header("SoundData")]
	[Range(0,1)]
	public float masterVol;
	[Range(0,1)]
	public float bgmVol;
	[Range(0,1)]
	public float sfxVol;
}
