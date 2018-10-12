using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public float ForceAmount;

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Rigidbody2D ballBody = collision.collider.GetComponent<Rigidbody2D>();

            if (ballBody)
            {
                //Debug.Log("Ball hit bumper");
                ballBody.AddForceAtPosition(-collision.contacts[0].normal * collision.relativeVelocity.magnitude * ForceAmount * ballBody.mass, collision.contacts[0].point);
            }
        }
        else if (collision.collider.CompareTag("Character"))
        {

        }
    }
}
