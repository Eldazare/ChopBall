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

	public static void SetDefaultsAll(){
		LoadInputStorages ();
		foreach (InputStorage storage in storages) {
			storage.ReadDefaultButtonsFromCurrentModel ();
		}
	}

	public static void SetDefault(int playerID){
		LoadInputStorages ();
		storages [playerID - 1].ReadDefaultButtonsFromCurrentModel ();
	}

	public static void SetModelToStorage(int playerID, string modelName){
		LoadInputStorages ();
		storages [playerID - 1].ReadModel (ControllerModelController.GetControllerModel (modelName));
	}
}
