using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Vector3 startPosition; // initialize this with loader, use in Reset method
	public GameObject preSpawnIndicator;
	public List<Vector3> startPositions;
	public List<int> touchedPlayers = new List<int>(16);

	MeshRenderer meshRenderer;
	CircleCollider2D circleCollider;
	TrailRenderer trailRenderer;
    BallGravity gravity;
    Rigidbody2D rigid2D;
	Gradient gradient;
	GradientAlphaKey[] gradAlphKey;


    private bool charged = false;

	private GameObject preSpawnIndicatorInstance;

	private string soundBall1Path;



	public void ResetBallPosition(){
		StartCoroutine (ResetEnumerator ());
		touchedPlayers.Clear ();
	}

	public void Initialize(List<Transform> ballSpawns){
		startPositions = new List<Vector3> ();
        if (ballSpawns != null)
        {
            foreach (var spawn in ballSpawns)
            {
                startPositions.Add(spawn.position);
            }
        }
        else {
            startPositions.Add(Vector3.zero);
        }
		soundBall1Path = SoundPathController.GetPath ("Ball1");
		preSpawnIndicatorInstance = Instantiate (preSpawnIndicator, Vector3.zero, Quaternion.identity);
		preSpawnIndicatorInstance.SetActive (false);

		meshRenderer = GetComponent<MeshRenderer>();
		circleCollider = GetComponent<CircleCollider2D>();
		trailRenderer = GetComponentInChildren<TrailRenderer>();
        gravity = GetComponentInChildren<BallGravity>();
        rigid2D = GetComponent<Rigidbody2D> ();
		gradient = new Gradient();
		gradAlphKey = new GradientAlphaKey[] { new GradientAlphaKey(1f,0f), new GradientAlphaKey(0f,1f) };
		gradient.SetKeys (
			new GradientColorKey[] {new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
			gradAlphKey
		);
	}

	public void GetPlayerPaddleTouch(int playerID,  GradientColorKey[] colorKeys, bool chargeShot = false){
		if (touchedPlayers.Contains(playerID)) {
			touchedPlayers.Remove (playerID);
		}
		touchedPlayers.Insert (0, playerID);
        charged = chargeShot;
		gradient.SetKeys (
			colorKeys,
			gradAlphKey
		);

		/*
		gradient.SetKeys (
			new GradientColorKey[] {new GradientColorKey(color, 0f),new GradientColorKey(color, 1f) },
			gradAlphKey
		);
		*/

		trailRenderer.colorGradient = gradient;
		FMODUnity.RuntimeManager.PlayOneShot (soundBall1Path, gameObject.transform.position);
    }

	private IEnumerator ResetEnumerator(){
		Vector3 spawnPos;
		if (startPositions != null) {
			spawnPos = startPositions [Random.Range (0, startPositions.Count)];
		} else {
			spawnPos = startPosition;
		}
		meshRenderer.enabled = false;
		circleCollider.enabled = false;
		trailRenderer.enabled = false;
        gravity.enabled = false;
        rigid2D.velocity = Vector2.zero;
		preSpawnIndicatorInstance.transform.position = spawnPos;
		preSpawnIndicatorInstance.SetActive (true);
		yield return new WaitForSeconds(3f);
		preSpawnIndicatorInstance.SetActive(false);
		transform.position = spawnPos;
		meshRenderer.enabled = true;
		circleCollider.enabled = true;
		trailRenderer.enabled = true;
        gravity.enabled = true;
    }
}
