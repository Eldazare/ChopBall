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

	public static void SetAButtonToStorage(int playerID, ButtonCommand command, KeyCode newButton){
		LoadInputStorages ();
		storages [playerID - 1].ChangeAButton (command, newButton);
	}

	public static string GetAStorageAsString(int playerID){
		LoadInputStorages ();
		InputStorage storage = storages [playerID - 1];
		string str = string.Format ("{0,-8} {5,-8} \n" +
			"{1,-8} {6,-8} \n" +
			"{2,-8} {7,-8} \n" +
			"{3,-8} {8,-8} \n" +
			"{4,-8} {9,-8} \n", 
			"Dash", "PaddleLeft", "PaddleRight", "Submit", "Cancel",
			storage.Dash.ToString (), storage.PaddleLeft.ToString (), storage.PaddleRight.ToString (),
			storage.Submit.ToString (), storage.Cancel.ToString ());
		return str;
	}
}
