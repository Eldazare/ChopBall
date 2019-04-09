using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMenuControllerChecker : MonoBehaviour {


    public InputTranslatorMaster master;

	void Start () {
        master.StartChecking();
	}

    public void EndChecking() {
        master.StopChecking();
    }
}
