using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Vector3 startPosition; // initialize this with loader, use in Reset method
	public GameObject preSpawnIndicator;
	public List<Vector3> startPositions;
	public List<int> touchedPlayers = new List<int>(16);

	private GameObject preSpawnIndicatorInstance;


	public void ResetBallPosition(){
		StartCoroutine (ResetEnumerator ());
		touchedPlayers.Clear ();
	}

	public void Initialize(List<Transform> ballSpawns){
		startPositions = new List<Vector3> ();
		foreach (var spawn in ballSpawns) {
			startPositions.Add (spawn.position);
		}
		preSpawnIndicatorInstance = Instantiate (preSpawnIndicator, Vector3.zero, Quaternion.identity);
		preSpawnIndicatorInstance.SetActive (false);
	}

	public void GetPlayerPaddleTouch(int playerID){
		if (touchedPlayers.Contains(playerID)) {
			touchedPlayers.Remove (playerID);
		}
		touchedPlayers.Insert (0, playerID);
	}

	private IEnumerator ResetEnumerator(){
		Vector3 spawnPos;
		if (startPositions != null) {
			spawnPos = startPositions [Random.Range (0, startPositions.Count)];
		} else {
			spawnPos = startPosition;
		}
		GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<CircleCollider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		preSpawnIndicatorInstance.transform.position = spawnPos;
		preSpawnIndicatorInstance.SetActive (true);
		yield return new WaitForSeconds(3f);
		preSpawnIndicatorInstance.SetActive(false);
		transform.position = spawnPos;
		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}
}
