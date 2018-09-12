using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class InputStorageController {

	private static InputStorage[] storages;

	private static void LoadInputStorages(){
		if (storages == null) {
			storages = Resources.LoadAll ("Scriptables/Input/ButtonStorage", typeof(InputStorage)).Cast<InputStorage> ().ToArray ();
		}
	}

	public static InputStorage[] GetAllStorages(){
		LoadInputStorages ();
		return storages;
	}

	public static InputStorage GetAStorage(int playerID){
		LoadInputStorages ();
		return storages [playerID - 1];
	}
}
