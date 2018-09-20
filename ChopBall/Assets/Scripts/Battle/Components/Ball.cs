using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {


	public int lastTouchedPlayerID; // Manipulate this directly: Player sets, Goal gets
	public Vector3 startPosition; // initialize this with loader, use in Reset method

	public void ResetBallPosition(){
		//TODO: Move / disable ball so that it enters play from the center
	}
}
