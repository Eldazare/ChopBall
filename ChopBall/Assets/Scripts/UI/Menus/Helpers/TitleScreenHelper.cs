using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenHelper : MonoBehaviour {

	public MasterCursor masterCursor;

	void OnEnable(){
		masterCursor.EnabelAllListeners ();
	}
}
