using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValueCaller : MonoBehaviour {

	void Start(){
		InputStorage[] storages = Resources.FindObjectsOfTypeAll<InputStorage> ();
		if (storages == null) {
			Debug.LogWarning ("Input storages not found");
		} else {
			foreach (InputStorage stor in storages) {
				stor.GetDefaultButtons ();
			}
		}
	}
}
