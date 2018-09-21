using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPaddle : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private float length = 0.5f;
    [SerializeField]
    private float startRotation;
    [SerializeField]
    private float endRotation;
    [SerializeField]
    private Transform pivot;
    [SerializeField]
    private LayerMask collisionLayers;
    [SerializeField]
    private float addForceAmount;

    private float currentRotation;
    private float targetRotation;
    private float currentAngularDirection;
    private float paddleHitDirection;
    private Vector2 paddleVector;
    private Vector2 pivotPoint;
    private Transform masterTransform;

    private bool hitActive = false;
    private float hitElapsed = 0f;

    private Collider2D[] colliderBuffer = new Collider2D[16];
    private List<int> hitObjectIDs = new List<int>(16);

    private void Awake()
    {
        masterTransform = transform.parent;
        if (!masterTransform) masterTransform = transform;

        if (!pivot) pivot = transform;
        pivotPoint = pivot.position;

        currentRotation = startRotation;
        targetRotation = currentRotation;
        paddleVector = Rotate(masterTransform.up, startRotation);

        paddleHitDirection = Mathf.Sign(endRotation - startRotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Hit();
        }
    }

    private void FixedUpdate()
    {
        UpdatePaddle();
    }

    public void Hit()
    {
        if (!hitActive)
        {
            currentAngularDirection = 1;
            hitActive = true;
        }
    }

    // Updates the paddles state and transform
    private void UpdatePaddle()
    {
        pivotPoint = pivot.position;

        if (hitActive)
        {
            hitElapsed += speed * currentAngularDirection * Time.deltaTime;

            if (hitElapsed >= 1)
                currentAngularDirection = -1;
            else if (hitElapsed <= 0 && currentAngularDirection == -1)
            {
                hitElapsed = 0;
                hitActive = false;
                hitObjectIDs.Clear();
                return;
            }

            targetRotation = Mathf.Lerp(startRotation, endRotation, hitElapsed);

            CheckPaddleCollisions();
        }

        paddleVector = Rotate(masterTransform.up, targetRotation);
        currentRotation = targetRotation;

        Debug.DrawLine(pivotPoint, pivotPoint + paddleVector * length, Color.green);
    }

    // Checks if paddle has collided with anything in collision layers
    private void CheckPaddleCollisions()
    {
        // Check if there are any colliders in the paddle radius -- radius is measured from the pivotpoint of the paddle
        int colliderCount = Physics2D.OverlapCircleNonAlloc(pivotPoint, length, colliderBuffer, collisionLayers);

        // If any colliders are found -> proceed
        if (colliderCount > 0)
        {
            // Go through all of the colliders
            for (int i = 0; i < colliderCount; i++)
            {
                int hitObjectID = colliderBuffer[i].GetInstanceID();
                // If object has been hit already we skip it
                if (hitObjectIDs.Contains(hitObjectID))
                {
                    continue;
                }

                // Check for a rigidbody on the object
                Rigidbody2D hitBody = colliderBuffer[i].GetComponent<Rigidbody2D>();

                // If rigidbody is found -> proceed
                if (hitBody)
                {
                    // The direction from the pivot to the object
                    Vector2 directionFromPivot = (hitBody.position - pivotPoint).normalized;

                    // Calculate the hit normal based on the direction of the hit
                    Vector2 hitNormal = new Vector2(-paddleVector.y, paddleVector.x).normalized * paddleHitDirection;

                    // Do a raycast check in the direction of the paddle to see if the object hit the side of the paddle
                    RaycastHit2D paddleSideHit = Physics2D.Raycast(pivotPoint, paddleVector, length, collisionLayers);

                    if (paddleSideHit)
                    {
                        // If object hit the side of the paddle we need to check if it hit the tip or not
                        Vector2 tipPoint = pivotPoint + paddleVector * length;
                        if (colliderBuffer[i].OverlapPoint(tipPoint))
                        {
                            // If object hit the tip we calculate the normal differently
                            hitNormal = (hitBody.position - tipPoint).normalized;
                        }
                    }
                    else
                    {
                        // If the object is not in the hit sector at all -> return
                        if (Vector2.Angle(-masterTransform.right, directionFromPivot) > currentRotation
                            || Vector2.SignedAngle(-masterTransform.right, directionFromPivot) < startRotation) return;

                        // If the object doesn't hit the side then the normal is just facing outwards from the pivot
                        hitNormal = directionFromPivot;
                    }

                    hitBody.AddForce(hitNormal * addForceAmount * hitBody.mass, ForceMode2D.Impulse);

                    hitObjectIDs.Add(hitObjectID);
                }
            }
        }
    }

    // Debug gizmos
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0, 0, 0, 0.5f);
    //    Gizmos.DrawSphere(pivotPoint, length);
    //    Gizmos.DrawSphere(pivotPoint + paddleVector * length, 0.1f);
    //}

    // Helper function to rotate a 2d vector -> change to an extension method later
    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
