using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour {

	private AudioSource[] soundSources;

	void Awake(){
		soundSources = GetComponents<AudioSource> ();
	}

	public void DoBGM(SoundInfo info){
		soundSources [0].Stop ();
		soundSources [0].clip = (AudioClip) Resources.Load ("Sound/" + info.command);
		soundSources [0].Play ();
	}

	public void DoSoundEffect(SoundInfo info){
		int randIndex = Random.Range (1, soundSources.Length);
		soundSources [randIndex].clip = (AudioClip) Resources.Load ("Sound/" + info.command);
		soundSources [randIndex].Play ();
	}
}
