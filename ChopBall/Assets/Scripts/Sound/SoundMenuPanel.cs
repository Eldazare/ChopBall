using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SoundMenuPanel : MonoBehaviour {

	public float min = 0;
	public float max = 100;
	Bus masterBus;
	Bus musicBus;
	Bus sfxBus;

	void Awake(){
		masterBus = FMODUnity.RuntimeManager.GetBus ("bus:/Master");
		musicBus = FMODUnity.RuntimeManager.GetBus ("bus:/Master/Music");
		sfxBus = FMODUnity.RuntimeManager.GetBus ("bus:/Master/Sounds");

		SliderConButton[] sliders = GetComponentsInChildren<SliderConButton> ();
		float masterVol = 0; 
		float sfxVol = 0;
		float musicVol = 0;
		float final = 0;
		masterBus.getVolume (out masterVol, out final);
		musicBus.getVolume (out musicVol, out final);
		sfxBus.getVolume (out sfxVol, out final);

		sliders [0].Initialize (min, max, masterVol*100, SetMasterVolume);
		sliders [1].Initialize (min, max, musicVol*100, SetMusicVolume);
		sliders [2].Initialize (min, max, sfxVol*100, SetSfxVolume);
	}

	public void SetMasterVolume(float value){
		masterBus.setVolume (value / 100);
	}

	public void SetMusicVolume(float value){
		musicBus.setVolume (value / 100);
	}

	public void SetSfxVolume(float value){
		sfxBus.setVolume (value / 100);
	}
}
