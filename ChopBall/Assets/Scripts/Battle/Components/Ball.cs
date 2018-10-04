using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Vector3 startPosition; // initialize this with loader, use in Reset method
	public List<int> touchedPlayers = new List<int>(16);

	public void ResetBallPosition(){
		StartCoroutine (ResetEnumerator ());
		touchedPlayers.Clear ();
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
		transform.position = startPosition;
		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<CircleCollider2D> ().enabled = true;
	}
}
