using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour {

	private Rigidbody rig;
	private InputModel model;

	public void GetInputModel(InputModel iModel){
		this.model = iModel;
	}

	void Awake(){
		rig = gameObject.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (model != null) {
			rig.velocity = model.rightDirectionalInput;
			if (model.Submit) {
				Debug.Log ("Submit registered");
			}
			if (model.Cancel) {
				Debug.Log ("Cancel registered");
			}
			if (model.Dash) {
				Debug.Log ("Dash registered");
			}
			if (model.Strike) {
				Debug.Log ("Strike registered");
			}
		}	
	}
}
