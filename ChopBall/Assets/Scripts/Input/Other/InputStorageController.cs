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

	public static bool SetModelToStorage(int playerID, string modelName){
		LoadInputStorages ();
		ControllerModel model = ControllerModelController.GetControllerModel (modelName);
		if (model != null) {
			storages [playerID - 1].ReadModel (model);
			return true;
		} else {
			return false;
		}
	}

	public static void SetAButtonToStorage(int playerID, ButtonCommand command, KeyCode newButton){
		LoadInputStorages ();
		storages [playerID - 1].ChangeAButton (command, newButton);
	}

	public static string GetAStorageAsString(int playerID){
		LoadInputStorages ();
		InputStorage storage = storages [playerID - 1];
		string str = string.Format ("Player {0} buttons:\n" +
			"{1,-20} {8,-8} \n" +
			"{2,-20} {9,-8} \n" +
			"{3,-20} {10,-8} \n" +
			"{4,-20} {11,-8} \n" +
			"{5,-20} {12,-8} \n" +
			"{6,-20} {13,-8} \n" +
			"{7,-20} {14,-8} \n", 
			playerID.ToString(),
			"Dash", "Strike", "Submit", "Cancel", "Start", "Select",
			storage.Dash.ToString () , storage.Strike.ToString (),
			storage.Submit.ToString (), storage.Cancel.ToString (), storage.Start.ToString(), storage.Select.ToString());
		return str;
	}
}
