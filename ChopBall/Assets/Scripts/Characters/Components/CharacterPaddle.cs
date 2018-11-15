using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CharacterPaddle : MonoBehaviour {

    public enum PaddleSide { Left = -1, Right = 1 }

    public PaddleSide Side = PaddleSide.Left;
    public Transform Pivot;

    internal bool isCharging = false;
    internal bool hitActive = false;

    private float currentRotation;
    private float targetRotation;
    private float currentAngularDirection;
    private float paddleHitDirection;
    private Vector2 paddleVector;
    private Vector2 pivotPoint;
    private Transform masterTransform;

    private float hitElapsed = 0f;
    private bool hitIsCharged = false;

    private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    private List<int> hitObjectIDs = new List<int>(16);

    private int playerID;
    private CharacterBaseData characterBase;
    private CharacterAttributeData characterAttributes;
	private CharacterRuntimeModifiers characterRuntimeModifiers;
	private GradientColorKey[] theColors;

	private string soundPaddlePath;
	private string soundHitPath;

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

	public void SetRuntimeModifiers(CharacterRuntimeModifiers runtimeMods){
		characterRuntimeModifiers = runtimeMods;
	}

	public void Initialize(Color32 theColor)
    {
        masterTransform = transform.parent;
        if (!masterTransform) masterTransform = transform;

        if (!Pivot) Pivot = transform;
        pivotPoint = Pivot.position;

		this.theColors = new GradientColorKey[]{new GradientColorKey(theColor, 0f), new GradientColorKey(theColor, 1f)};

        currentRotation = characterBase.PaddleLowerAngle * characterAttributes.PaddleLowerAngleMultiplier * (float)Side;
        targetRotation = currentRotation;
        paddleVector = Rotate(masterTransform.up, targetRotation);
        Pivot.localRotation = Quaternion.AngleAxis(currentRotation, transform.forward);

        paddleHitDirection = Mathf.Sign(characterBase.PaddleUpperAngle * characterAttributes.PaddleUpperAngleMultiplier - currentRotation);

		soundPaddlePath = SoundPathController.GetPath ("Chop");
		soundHitPath = SoundPathController.GetPath ("PlayerHit");
    }

	public void Hit(bool charged = false)
    {
        if (!hitActive || !hitIsCharged)
        {
			if (characterRuntimeModifiers.UseStamina (characterBase.PaddleStaminaCost)) {
				FMODUnity.RuntimeManager.PlayOneShotAttached (soundPaddlePath, gameObject);
				//soundEmitter.SendMessage("Play");
				hitObjectIDs.Clear ();
				hitElapsed = 0;
				hitIsCharged = charged;
				currentAngularDirection = 1;
				hitActive = true;
			}
        }
    }

    // Updates the paddles state and transform
    public void UpdatePaddle()
    {
        pivotPoint = Pivot.position;

		if (hitActive) {
			float paddleSpeed = (currentAngularDirection > 0) ?
                                characterBase.PaddleSpeedUp * characterAttributes.PaddleSpeedUpMultiplier : 
                                characterBase.PaddleSpeedDown * characterAttributes.PaddleSpeedDownMultiplier;

			hitElapsed += paddleSpeed * currentAngularDirection * Time.deltaTime;

			if (hitElapsed >= 1) {
				hitElapsed = 1;
				currentAngularDirection = -1;
			} else if (hitElapsed <= 0 && currentAngularDirection == -1) {
				hitElapsed = 0;
				hitActive = false;
                hitIsCharged = false;
			}

			targetRotation = Mathf.Lerp (characterBase.PaddleLowerAngle * characterAttributes.PaddleLowerAngleMultiplier * (float)Side,
				characterBase.PaddleUpperAngle * characterAttributes.PaddleUpperAngleMultiplier, hitElapsed);
			currentRotation = targetRotation;

			paddleVector = Rotate (masterTransform.up, targetRotation);
            Pivot.localRotation = Quaternion.AngleAxis(currentRotation, transform.forward);

            if (currentAngularDirection > 0)
				CheckPaddleCollisions ();

			//if (hitActive == false) hitObjectIDs.Clear();
		} else {
			paddleVector = Rotate (masterTransform.up, targetRotation);
		}

        //Debug.DrawLine(pivotPoint, pivotPoint + paddleVector * characterBase.PaddleLength, Color.green);
        //Debug.DrawLine(pivotPoint - (characterBase.PaddleThickness / 2) * paddleVector, pivotPoint + (characterBase.PaddleLength + characterBase.PaddleThickness / 2) * paddleVector, Color.green);
    }

    // Checks if paddle has collided with anything in collision layers
    private void CheckPaddleCollisions()
    {
        int paddleHits = Physics2D.CircleCastNonAlloc(pivotPoint,
                                                    (characterBase.PaddleThickness * characterAttributes.PaddleThicknessMultiplier) / 2,
                                                    paddleVector,
                                                    hitBuffer,
                                                    characterBase.PaddleLength * characterAttributes.PaddleLengthMultiplier,
                                                    characterBase.PaddleCollisionLayers);

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

                    if (!hitIsCharged)
                    {
                        Vector2 tipPoint = pivotPoint + paddleVector * characterBase.PaddleLength * characterAttributes.PaddleLengthMultiplier;
                        Vector2 distanceFromTip = (hitBody.position - tipPoint);

                        //Vector2 distanceFromPivot = (hitBody.position - pivotPoint);
                        //float distanceMultiplier = distanceFromPivot.magnitude;
                        //if (Vector2.Dot(distanceFromPivot, paddleVector) < 0) distanceMultiplier = 0;

                        float ballRadius = hitBody.GetComponent<CircleCollider2D>().radius * hitBuffer[i].transform.localScale.y;

                        if (distanceFromTip.magnitude <= ballRadius + (characterBase.PaddleThickness * characterAttributes.PaddleThicknessMultiplier) / 2 && Vector2.Dot(distanceFromTip, paddleVector) > 0f)
                        {
                            //Debug.Log("tip hit");
                            hitNormal = distanceFromTip.normalized;
                        }
                        else hitNormal = new Vector2(-paddleVector.y, paddleVector.x).normalized * paddleHitDirection;
                    }
                    else
                    {
                        Debug.Log("Charge shot");
                        hitNormal = masterTransform.up;
						hitNormal *= characterBase.PaddleChargedForceMultiplier;
                    }

                    //Debug.DrawRay(hitBuffer[i].point, hitNormal, Color.red, 1f);

                    hitBody.velocity = Vector2.zero;
                    hitBody.AddForce(hitNormal * characterBase.PaddleForceAmount * characterAttributes.PaddleForceMultiplier * hitBody.mass, ForceMode2D.Impulse);

                    Ball hitBall = hitBody.GetComponent<Ball>();
					if (hitBall) hitBall.GetPlayerPaddleTouch(playerID, theColors, hitIsCharged);

                    hitBody.GetComponentInChildren<BallGravity>().AddUpwardsVelocity(4f);

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
            Gizmos.DrawSphere(pivotPoint + ((characterBase.PaddleLength * characterAttributes.PaddleLengthMultiplier / 10) * i * paddleVector),
                                            characterBase.PaddleThickness * characterAttributes.PaddleThicknessMultiplier / 2);
        }

        Gizmos.color = new Color(0, 255, 0, 0.5f);

        Gizmos.DrawSphere(pivotPoint + characterBase.PaddleLength * characterAttributes.PaddleLengthMultiplier * paddleVector,
                          characterBase.PaddleThickness * characterAttributes.PaddleThicknessMultiplier / 2);
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
