using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {


	public int lastTouchedPlayerID; // Manipulate this directly: Player sets, Goal gets
	public Vector3 startPosition; // initialize this with loader, use in Reset method

	public void ResetBallPosition(){
		StartCoroutine (ResetEnumerator ());
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
