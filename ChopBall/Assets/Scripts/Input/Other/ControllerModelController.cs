using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ControllerModelController {

	private static ControllerModel[] controllerModels;

	public static void LoadControllerModels(){
		if (controllerModels == null) {
			controllerModels = Resources.LoadAll ("Scriptables/Input/ControllerModels").Cast<ControllerModel> ().ToArray ();
		}
	}

	public static ControllerModel GetControllerModel(string controllerModelName){
		LoadControllerModels ();
		foreach (ControllerModel cModel in controllerModels) {
			if (cModel.ControllerName == controllerModelName) {
				return cModel;
			}
		}
		Debug.LogWarning ("ControllerModel not found for: " + controllerModelName);
		return null;
	}
}
