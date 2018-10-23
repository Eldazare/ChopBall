using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public float BallForceAmount;
    public float BallMinForce;
    public float PlayerForceAmount;

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
                ballBody.AddForceAtPosition(-collision.contacts[0].normal * ballBody.mass * appliedForce, collision.contacts[0].point);
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
    }
}
