using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public float BallForceAmount;
    public float BallMinForce;
    public float PlayerForceAmount;

    private Animator animator;

	private string soundBumper1Path;

    private void Awake()
    {
        animator = GetComponent<Animator>();
		soundBumper1Path = SoundPathController.GetPath ("Bumper1");
    }

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Rigidbody2D ballBody = collision.collider.GetComponent<Rigidbody2D>();

            if (ballBody)
            {
                //Debug.Log("Ball hit bumper");
                float appliedForce = collision.relativeVelocity.magnitude * BallForceAmount;
                appliedForce = Mathf.Clamp(appliedForce, BallMinForce, Mathf.Infinity);

                ballBody.velocity = Vector2.zero;
                ballBody.AddForce(-collision.contacts[0].normal * ballBody.mass * appliedForce, ForceMode2D.Impulse);
                ballBody.GetComponentInChildren<BallGravity>().AddUpwardsVelocity(4f);
            }
        }
        else if (collision.collider.CompareTag("Character"))
        {
            CharacterMovement charMovement = collision.collider.GetComponent<CharacterMovement>();

            if (charMovement)
            {
                charMovement.AddForce(-collision.contacts[0].normal * PlayerForceAmount);
            }
        }

        animator.Play("Bump");
		FMODUnity.RuntimeManager.PlayOneShotAttached (soundBumper1Path, gameObject);
    }
}
