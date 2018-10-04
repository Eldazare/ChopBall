﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Vector3 startPosition; // initialize this with loader, use in Reset method
	public List<Vector3> startPositions;
	public List<int> touchedPlayers = new List<int>(16);


	public void ResetBallPosition(){
		StartCoroutine (ResetEnumerator ());
		touchedPlayers.Clear ();
	}

	public void Initialize(Transform[] ballSpawns){
		startPositions = new List<Vector3> ();
		foreach (var spawn in ballSpawns) {
			startPositions.Add (spawn.position);
		}
	}

	public void GetPlayerPaddleTouch(int playerID){
		if (touchedPlayers.Contains(playerID)) {
			touchedPlayers.Remove (playerID);
		}
		touchedPlayers.Insert (0, playerID);
	}

	private IEnumerator ResetEnumerator(){
		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<CircleCollider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		yield return new WaitForSeconds(3f);
		if (startPositions != null) {
			transform.position = startPositions [Random.Range (0, startPositions.Count)];
		} else {
			transform.position = startPosition;
		}
		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}
}
