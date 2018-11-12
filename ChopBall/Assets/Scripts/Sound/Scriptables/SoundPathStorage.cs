using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class SoundPathStorage : ScriptableObject {

	public List<SoundPathData> pathData;
}


[Serializable]
public class SoundPathData{
	public string key;

	[FMODUnity.EventRef]
	public string path;
}
