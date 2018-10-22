﻿using System.Collections;
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

    private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
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
            float paddleSpeed = (currentAngularDirection > 0) ? characterBase.PaddleSpeedUp : characterBase.PaddleSpeedDown;

            hitElapsed += paddleSpeed * currentAngularDirection * Time.deltaTime;

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
            currentRotation = targetRotation;

            paddleVector = Rotate(masterTransform.up, targetRotation);

            if (currentAngularDirection > 0) CheckPaddleCollisions();

            if (hitActive == false) hitObjectIDs.Clear();
        }
        else paddleVector = Rotate(masterTransform.up, targetRotation);

        //Debug.DrawLine(pivotPoint, pivotPoint + paddleVector * characterBase.PaddleLength, Color.green);
        //Debug.DrawLine(pivotPoint - (characterBase.PaddleThickness / 2) * paddleVector, pivotPoint + (characterBase.PaddleLength + characterBase.PaddleThickness / 2) * paddleVector, Color.green);
    }

    // Checks if paddle has collided with anything in collision layers
    private void CheckPaddleCollisions()
    {
        int paddleHits = Physics2D.CircleCastNonAlloc(pivotPoint, characterBase.PaddleThickness / 2, paddleVector, hitBuffer, characterBase.PaddleLength, characterBase.PaddleCollisionLayers);

        // If any colliders are found -> proceed
        if (paddleHits > 0)
        {
            // Go through all of the hits
            for (int i = 0; i < paddleHits; i++)
            {
                int hitObjectID = hitBuffer[i].collider.GetInstanceID();
                // If object has been hit already we skip it
                if (hitObjectIDs.Contains(hitObjectID))
                {
                    continue;
                }

                //Debug.Log("Colliders found");

                // Check for a rigidbody on the object
                Rigidbody2D hitBody = hitBuffer[i].collider.GetComponent<Rigidbody2D>();

                if (hitBody)
                {
                    //Debug.Log("Rigidbody found");

                    // Calculate the hit normal based on the direction of the hit
                    Vector2 hitNormal;

                    Vector2 tipPoint = pivotPoint + paddleVector * characterBase.PaddleLength;
                    Vector2 distanceFromTip = (hitBody.position - tipPoint);

                    //Vector2 distanceFromPivot = (hitBody.position - pivotPoint);
                    //float distanceMultiplier = distanceFromPivot.magnitude;
                    //if (Vector2.Dot(distanceFromPivot, paddleVector) < 0) distanceMultiplier = 0;

                    float ballRadius = hitBody.GetComponent<CircleCollider2D>().radius * hitBuffer[i].transform.localScale.y;

                    if (distanceFromTip.magnitude <= ballRadius + characterBase.PaddleThickness / 2 && Vector2.Dot(distanceFromTip, paddleVector) > 0f)
                    {
                        //Debug.Log("tip hit");
                        hitNormal = distanceFromTip.normalized;
                    }
                    else hitNormal = new Vector2(-paddleVector.y, paddleVector.x).normalized * paddleHitDirection;

                    //Debug.DrawRay(hitBuffer[i].point, hitNormal, Color.red, 1f);

                    hitBody.velocity = Vector2.zero;
                    hitBody.AddForceAtPosition(hitNormal * characterBase.PaddleForceAmount * hitBody.mass, hitBody.position - hitNormal * ballRadius, ForceMode2D.Impulse);

                    Ball hitBall = hitBody.GetComponent<Ball>();
                    if (hitBall) hitBall.GetPlayerPaddleTouch(playerID);

                    hitObjectIDs.Add(hitObjectID);
                }
            }
        }
    }

    //Debug gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 255, 255, 0.5f);

        for (int i = 0; i < 10; i++)
        {
            Gizmos.DrawSphere(pivotPoint + ((characterBase.PaddleLength / 10) * i * paddleVector), characterBase.PaddleThickness / 2);
        }

        Gizmos.color = new Color(0, 255, 0, 0.5f);

        Gizmos.DrawSphere(pivotPoint + characterBase.PaddleLength * paddleVector, characterBase.PaddleThickness / 2);
    }

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
