using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettingsLoader : MonoBehaviour {

	void Awake(){

		// Loading sound volumes
		SystemSettings sett = (SystemSettings) Resources.Load ("Scriptables/_BaseDatas/SystemSettings");
		FMODUnity.RuntimeManager.GetBus ("bus:/Master").setVolume(sett.masterVol);
		FMODUnity.RuntimeManager.GetBus ("bus:/Master/Music").setVolume(sett.bgmVol);
		FMODUnity.RuntimeManager.GetBus ("bus:/Master/Sounds").setVolume(sett.sfxVol);
	}
}
