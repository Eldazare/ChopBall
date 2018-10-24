using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour {

	void Start () {
		BuffController.AddBuff (1, new MovespeedBuff (5.0f, 0.5f));	
	}
}
