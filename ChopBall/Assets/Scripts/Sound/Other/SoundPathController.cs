using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SoundPathController {

	private static List<SoundPathStorage> storages;
	private static Dictionary<string,string> data;

	private static void LoadSoundPaths(){
		if (storages == null) {
			storages = Resources.LoadAll ("Scriptables/Sound/", typeof(SoundPathStorage)).Cast<SoundPathStorage>().ToList();
			data = new Dictionary<string, string> ();
			foreach (var storage in storages) {
				foreach (var listObj in storage.pathData) {
					data.Add (listObj.key, listObj.path);
				}
			}
			// TODO: Check for duplicate keys / paths?
		}
	}

	public static string GetPath(string key){
		LoadSoundPaths ();
		if (!data.ContainsKey(key)) {
			Debug.LogWarning ("SoundPathStorage no key found: " + key);
			return "";
		}
		return data[key];
	}

	public static string[] GetKeys(){
		LoadSoundPaths ();
		return data.Keys.ToArray ();
	}
}
