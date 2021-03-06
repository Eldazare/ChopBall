﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneReturnHelper : MonoBehaviour {


	public GameObject endIndicator;
	/*
	void Start () {
		StartCoroutine(ReturnToMenu (5.0f));
	}
	*/
	
	private IEnumerator ReturnToMenu(float delay){
		Debug.Log ("Returning to menu in " + delay + " seconds...");
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene ("MenuScene2");
	}


	public void GetInput(InputModel model){
		if (model.Start) {
			endIndicator.SetActive (true);
			SceneManager.LoadScene ("MenuScene2");
		}
	}
}
