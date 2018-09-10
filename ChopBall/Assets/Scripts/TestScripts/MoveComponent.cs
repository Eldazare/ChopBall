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
			rig.velocity = new Vector3 (model.XAxisLeft, model.YAxisLeft);
			if (model.Submit) {
				Debug.Log ("Submit registered");
			}
		}	
	}
}
