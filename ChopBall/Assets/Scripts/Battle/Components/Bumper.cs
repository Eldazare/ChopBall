using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public float ForceAmount;
    public float MinForce;

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Rigidbody2D ballBody = collision.collider.GetComponent<Rigidbody2D>();

            if (ballBody)
            {
                //Debug.Log("Ball hit bumper");
                float appliedForce = collision.relativeVelocity.magnitude * ForceAmount;
                appliedForce = Mathf.Clamp(appliedForce, MinForce, Mathf.Infinity);

                ballBody.velocity = Vector2.zero;
                ballBody.AddForceAtPosition(-collision.contacts[0].normal * ballBody.mass * appliedForce, collision.contacts[0].point);
            }
        }
        //else if (collision.collider.CompareTag("Character"))
        //{

        //}
    }
}
