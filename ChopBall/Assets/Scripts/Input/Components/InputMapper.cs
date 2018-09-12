using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMapper : MonoBehaviour {

	// FLOW: ? 
	// Generate buttons
	// Wait for button message
	// Prime yourself for an input (Custom translator which has all the buttons) 
	// (Maybe disable translator for the time being)
	// Run custom translator until input


	List<InputModel> models;

	void Initialize(){
		models = new List<InputModel> ();
		int inputAmount = InputStorageController.GetAllStorages ().Length;
		for (int i = 0; i < inputAmount; i++) {
			models.Add (new InputModel ());
		}
	}

	public void ReceiveInputModel(InputModel model){
		models [model.playerID] = model;
	}
}
