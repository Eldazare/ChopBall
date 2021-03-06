﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Vector3 startPosition; // initialize this with loader, use in Reset method
	public GameObject preSpawnIndicator;
	public List<Vector3> startPositions;
	public List<int> touchedPlayers = new List<int>(16);
    public ParticleSystem ChargedParticles;

    public bool HasCollidedWithTarget { get; set; }

    MeshRenderer meshRenderer;
	CircleCollider2D circleCollider;
	TrailRenderer trailRenderer;
    BallGravity gravity;
    //Projector blobShadow;
    Rigidbody2D rigid2D;
	Gradient gradient;
	GradientAlphaKey[] gradAlphKey;


    private bool charged = false;

	private GameObject preSpawnIndicatorInstance;

	private string soundBall1Path;

	private bool paused = false;
	private Vector2 savedVelocity = Vector2.zero;

    private void OnEnable()
    {
        CinemachineTargetGroup targetGroup = GameObject.Find("Camera Target Group").GetComponent<CinemachineTargetGroup>();
        if (targetGroup != null)
        {
            for (int i = 0; i < targetGroup.m_Targets.Length; i++)
            {
                if (targetGroup.m_Targets[i].target == null)
                {
                    targetGroup.m_Targets[i].target = transform;
                    targetGroup.m_Targets[i].weight = 1f;
                    targetGroup.m_Targets[i].radius = 1f;
                    return;
                }
            }
        }
    }

    private void OnDisable()
    {
        CinemachineTargetGroup targetGroup = GameObject.Find("Camera Target Group").GetComponent<CinemachineTargetGroup>();
        if (targetGroup != null)
        {
            for (int i = 0; i < targetGroup.m_Targets.Length; i++)
            {
                if (targetGroup.m_Targets[i].target == transform)
                {
                    targetGroup.m_Targets[i].target = null;
                    targetGroup.m_Targets[i].weight = 0f;
                    targetGroup.m_Targets[i].radius = 0f;
                    return;
                }
            }
        }
    }

    public void Pause(){
		if (paused) {
			paused = false;
			rigid2D.velocity = savedVelocity;
			savedVelocity = Vector2.zero;
		} else {
			paused = true;
			savedVelocity = rigid2D.velocity;
			rigid2D.velocity = Vector2.zero;
		}
	}

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

		meshRenderer = GetComponentInChildren<MeshRenderer>();
		circleCollider = GetComponent<CircleCollider2D>();
		trailRenderer = GetComponentInChildren<TrailRenderer>();
        gravity = GetComponentInChildren<BallGravity>();
        //blobShadow = GetComponentInChildren<Projector>();
        rigid2D = GetComponent<Rigidbody2D> ();
		gradient = new Gradient();
		gradAlphKey = new GradientAlphaKey[] { new GradientAlphaKey(1f,0f), new GradientAlphaKey(0f,1f) };
		gradient.SetKeys (
			new GradientColorKey[] {new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
			gradAlphKey
		);
        SetChargedStatus(false);
    }

    public void GetPlayerPaddleTouch(int playerID,  GradientColorKey[] colorKeys, bool chargeShot = false){
		if (touchedPlayers.Contains(playerID)) {
			touchedPlayers.Remove (playerID);
		}
		touchedPlayers.Insert (0, playerID);
        SetChargedStatus(chargeShot);
        gradient.SetKeys (
			colorKeys,
			gradAlphKey
		);
		trailRenderer.colorGradient = gradient;
		FMODUnity.RuntimeManager.PlayOneShot (soundBall1Path, gameObject.transform.position);
    }

    public void ResetTargetHit()
    {
        StartCoroutine(ResetTargetHitENumerator());
    }

    private IEnumerator ResetTargetHitENumerator()
    {
        HasCollidedWithTarget = true;
        yield return new WaitForSeconds(0.1f);
        HasCollidedWithTarget = false;
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
        //blobShadow.enabled = false;
        SetChargedStatus(false);
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
        //blobShadow.enabled = true;
    }

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.CompareTag("Wall")){
            SetChargedStatus(false);
        }
    }

	public void OnBlocked(Vector2 normal, float blockModifier){
        SetChargedStatus(false);
		Vector2 velo = rigid2D.velocity;
		//Debug.LogWarning ("Mag: " + velo.magnitude);
		rigid2D.velocity = Vector2.zero;
		rigid2D.AddForce (Vector2.Reflect (velo.normalized, normal) * rigid2D.mass * velo * blockModifier, ForceMode2D.Impulse);
	}

	public bool IsCharged(){
		return charged;
	}

    public void SetChargedStatus(bool charged)
    {
        this.charged = charged;
        if (charged)
        {
            ChargedParticles.Play();
        }
        else ChargedParticles.Stop();
    }
}
