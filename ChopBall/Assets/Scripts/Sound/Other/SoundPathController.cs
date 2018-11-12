using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SoundPathController {

	private static SoundPathStorage storage;

	private static void LoadSoundPaths(){
		if (storage == null) {
			storage = (SoundPathStorage)Resources.Load ("Scriptables/Sound/SoundPathStorage/", typeof(SoundPathStorage));
			// TODO: Check for duplicate keys / paths?
		}
	}

	public static string GetPath(string key){
		LoadSoundPaths ();
		SoundPathData data = storage.pathData.SingleOrDefault (s=> s.key == key);
		if (data == null) {
			Debug.LogWarning ("SoundPathStorage null data found with key: " + key);
			return "";
		}
		return data.path;
	}
}
