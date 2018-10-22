using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenHelper : MonoBehaviour {

	public GameObject masterCursor;

	void OnEnable(){
		masterCursor.GetComponent<MasterCursor> ().EnabelAllListeners ();
	}
}
