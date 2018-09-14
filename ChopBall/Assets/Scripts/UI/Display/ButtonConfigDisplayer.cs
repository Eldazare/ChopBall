using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfigDisplayer : MonoBehaviour {

	public int playerID;
	public Text text;

	void Update(){
		text.text = InputStorageController.GetAStorageAsString (playerID);
	}
}
