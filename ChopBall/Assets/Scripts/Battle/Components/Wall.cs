using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	private string soundWallPath;

	void Awake(){
		soundWallPath = SoundPathController.GetPath ("Wall");
	}

	void OnCollisionEnter2D(Collision2D collision){
		foreach(var contact in collision.contacts){
			if (collision.collider.CompareTag("Ball")){
				FMODUnity.RuntimeManager.PlayOneShot (soundWallPath, contact.point);
			}
		}
	}
}
