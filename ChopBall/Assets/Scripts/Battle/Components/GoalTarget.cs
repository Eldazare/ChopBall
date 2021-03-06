﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTarget : MonoBehaviour {
    
    public float MinBreakVelocity;
    public float BallForceAmount;
    public float BallMinForce;

    private Collider2D targetCollider;
    //private SpriteRenderer sprite;
	private MeshRenderer meshRenderer;

	private string soundTarget1Path;

    public void Activate()
    {
        targetCollider.enabled = true;
		meshRenderer.enabled = true;
    }

    public void DeActivate()
    {
        targetCollider.enabled = false;
		meshRenderer.enabled = false;
    }

    private void Awake()
    {
        targetCollider = GetComponent<Collider2D>();
		meshRenderer = GetComponentInChildren<MeshRenderer> ();
		soundTarget1Path = SoundPathController.GetPath ("Target1");
    }

	/*
    private void Update()
    {
        if (RespawnTime > 0f)
        {
            if (t > 0f) t -= Time.deltaTime;
            else if (!active)
            {
                targetCollider.enabled = true;
                sprite.enabled = true;
            }
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
			Ball ball = collision.collider.GetComponent<Ball> ();
            if (ball.HasCollidedWithTarget)
            {
                return;
            }

            if (collision.relativeVelocity.magnitude >= MinBreakVelocity)
            {
				DeActivate ();
            }

            Rigidbody2D ballBody = collision.collider.GetComponent<Rigidbody2D>();

            if (ballBody)
			{
				if (ball.IsCharged ()) {
					ContactPoint2D contact = collision.GetContact (0);

					float forceAmount = collision.relativeVelocity.magnitude * BallForceAmount;
					forceAmount = Mathf.Clamp (forceAmount, 0, BallMinForce*1.5f);
					Vector2 appliedForce = Vector2.Reflect (collision.relativeVelocity.normalized, contact.normal) * forceAmount;

					ballBody.velocity = Vector2.zero;
					ballBody.AddForce (ballBody.mass * appliedForce, ForceMode2D.Impulse);
				} else {
					ContactPoint2D contact = collision.GetContact (0);

					float forceAmount = collision.relativeVelocity.magnitude * BallForceAmount;
					forceAmount = Mathf.Clamp (forceAmount, BallMinForce, Mathf.Infinity);
					Vector2 appliedForce = Vector2.Reflect (collision.relativeVelocity.normalized, contact.normal) * forceAmount;

					ballBody.velocity = Vector2.zero;
					ballBody.AddForce (ballBody.mass * appliedForce, ForceMode2D.Impulse);
				}
            }

            FMODUnity.RuntimeManager.PlayOneShot (soundTarget1Path, gameObject.transform.position);

            collision.collider.GetComponent<Ball>().ResetTargetHit();
        }
    }
}
