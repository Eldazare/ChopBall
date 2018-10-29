using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfigDisplayer : MonoBehaviour {

	public int playerID;
	private Text text;


	void Awake(){
		text = GetComponent<Text> ();
	}

	void Update(){
		text.text = InputStorageController.GetAStorageAsString (playerID);
	}
}
