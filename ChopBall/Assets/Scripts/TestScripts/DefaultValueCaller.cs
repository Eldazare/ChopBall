using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValueCaller : MonoBehaviour {

	void Start(){
		InputStorage[] storages = Resources.LoadAll ("Scriptables/Input/Storages");
		foreach (InputStorage stor in storages) {
			stor.GetDefaultButtons ();
		}
	}
}
