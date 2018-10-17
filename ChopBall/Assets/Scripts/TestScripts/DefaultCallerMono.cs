using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCallerMono : MonoBehaviour {

	void Start(){
		InputStorageController.SetDefaultsAll ();
		PlayerStateController.SetDefaultsAll ();
		MasterStateController.GetTheMasterData ().SetMenuDefaults ();
	}
}
