using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPaddle : MonoBehaviour {

    public enum PaddleSide { Left = -1, Right = 1 }

    public PaddleSide Side = PaddleSide.Left;
    public Transform Pivot;

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

    private int playerID;
    private CharacterBaseData characterBase;
    private CharacterAttributeData characterAttributes;

    public void SetPlayerID(int id)
    {
        playerID = id;
    }

    public void SetCharacterBaseData(CharacterBaseData baseData)
    {
        characterBase = baseData;
    }

    public void SetCharacterAttributeData(CharacterAttributeData attributeData)
    {
        characterAttributes = attributeData;
    }

    private void Start()
    {
        masterTransform = transform.parent;
        if (!masterTransform) masterTransform = transform;

        if (!Pivot) Pivot = transform;
        pivotPoint = Pivot.position;

        currentRotation = characterBase.PaddleLowerAngle * (float)Side;
        targetRotation = currentRotation;
        paddleVector = Rotate(masterTransform.up, targetRotation);

        paddleHitDirection = Mathf.Sign(characterBase.PaddleUpperAngle - currentRotation);
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
    public void UpdatePaddle()
    {
        pivotPoint = Pivot.position;

        if (hitActive)
        {
            hitElapsed += characterBase.PaddleSpeedUp * currentAngularDirection * Time.deltaTime;

            if (hitElapsed >= 1)
            {
                hitElapsed = 1;
                currentAngularDirection = -1;
            }
            else if (hitElapsed <= 0 && currentAngularDirection == -1)
            {
                hitElapsed = 0;
                hitActive = false;
            }

            targetRotation = Mathf.Lerp(characterBase.PaddleLowerAngle * (float)Side, characterBase.PaddleUpperAngle, hitElapsed);

            CheckPaddleCollisions();

            if (hitActive == false) hitObjectIDs.Clear();
        }

        paddleVector = Rotate(masterTransform.up, targetRotation);
        currentRotation = targetRotation;

        Debug.DrawLine(pivotPoint, pivotPoint + paddleVector * characterBase.PaddleLength, Color.green);
    }

    // Checks if paddle has collided with anything in collision layers
    private void CheckPaddleCollisions()
    {
        // Check if there are any colliders in the paddle radius -- radius is measured from the pivotpoint of the paddle
        int colliderCount = Physics2D.OverlapCircleNonAlloc(pivotPoint,
                                                            characterBase.PaddleLength,
                                                            colliderBuffer,
                                                            characterBase.PaddleCollisionLayers);

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
                    RaycastHit2D paddleSideHit = Physics2D.Raycast(pivotPoint, paddleVector, characterBase.PaddleLength, characterBase.PaddleCollisionLayers);

                    if (paddleSideHit)
                    {
                        // If object hit the side of the paddle we need to check if it hit the tip or not
                        Vector2 tipPoint = pivotPoint + paddleVector * characterBase.PaddleLength;
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
                            || Vector2.SignedAngle(-masterTransform.right, directionFromPivot) < characterBase.PaddleLowerAngle * (float)Side) return;

                        // If the object doesn't hit the side then the normal is just facing outwards from the pivot
                        hitNormal = directionFromPivot;
                    }

                    hitBody.AddForce(hitNormal * characterBase.PaddleForceAmount * hitBody.mass, ForceMode2D.Impulse);

                    Ball hitBall = hitBody.GetComponent<Ball>();
					if (hitBall) hitBall.GetPlayerPaddleTouch(playerID);

                    hitObjectIDs.Add(hitObjectID);
                }
            }
        }
    }

    // Debug gizmos
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0, 0, 0, 0.5f);
    //    Gizmos.DrawSphere(pivotPoint, characterBase.PaddleLength);
    //    Gizmos.DrawSphere(pivotPoint + paddleVector * characterBase.PaddleLength, 0.1f);
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
