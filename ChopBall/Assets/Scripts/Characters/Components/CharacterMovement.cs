using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    internal bool isDashing = false;

    internal Vector2 velocity;
    internal float angularVelocity;

    private Vector2 appliedForce;

    private float rotationAnalogMultiplier;
    private Vector2 lookDirection;
    private Vector2 dashDirection;
    private float dashSpeed;
    private float dashTimerElapsed = 0f;
    private float dashCoolDownElapsed = 0f;

    private Rigidbody2D characterRigidbody;

    private CharacterBaseData characterBase;
    private CharacterAttributeData characterAttributes;

    public void SetCharacterBaseData(CharacterBaseData baseData)
    {
        characterBase = baseData;
    }

    public void SetCharacterAttributeData(CharacterAttributeData attributeData)
    {
        characterAttributes = attributeData;
    }

    public void ResetMovement()
    {
        isDashing = false;
        velocity = Vector2.zero;
        angularVelocity = 0f;
        characterRigidbody.velocity = Vector2.zero;
        characterRigidbody.angularVelocity = 0f;
        appliedForce = Vector2.zero;
        dashTimerElapsed = 0f;
    }

    public void SetPositionAndRotation(Vector2 position, float rotation)
    {
        ResetMovement();
        characterRigidbody.position = position;
        characterRigidbody.rotation = rotation;
    }

    private void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (dashCoolDownElapsed > 0)
        {
            dashCoolDownElapsed -= Time.deltaTime;
        }
        else dashCoolDownElapsed = 0;

        ApplyDrag();
    }

    public void Move(Vector2 inputAxis)
    {
        if (dashTimerElapsed <= 0)
        {
            isDashing = false;
			velocity.x = inputAxis.x * characterBase.MovementSpeed * characterAttributes.MovementSpeedMultiplier * characterRigidbody.drag * characterRigidbody.mass * Time.deltaTime;
			velocity.y = inputAxis.y * characterBase.MovementSpeed * characterAttributes.MovementSpeedMultiplier * characterRigidbody.drag * characterRigidbody.mass * Time.deltaTime;
        }
        else
        {
            isDashing = true;
            velocity.x = dashDirection.x * dashSpeed * characterRigidbody.drag * Time.deltaTime;
            velocity.y = dashDirection.y * dashSpeed * characterRigidbody.drag * Time.deltaTime;

            dashTimerElapsed -= Time.deltaTime;
        }

        velocity += appliedForce * characterRigidbody.drag * Time.deltaTime;

        characterRigidbody.AddForce(velocity - characterRigidbody.velocity, ForceMode2D.Impulse);
    }

    public void Rotate(Vector2 inputAxis)
    {
        rotationAnalogMultiplier = inputAxis.magnitude;
        if (rotationAnalogMultiplier > 0)
        {
            lookDirection.x = inputAxis.x;
            lookDirection.y = inputAxis.y;
        }

        float angle = Vector2.SignedAngle(transform.up, lookDirection);

        angularVelocity = characterBase.RotationSpeed * characterAttributes.RotationSpeedMultiplier * rotationAnalogMultiplier;

        angularVelocity = Mathf.Clamp(angularVelocity, 0, Mathf.Abs(angle)) * Mathf.Sign(angle);

        characterRigidbody.AddTorque((angularVelocity * Time.deltaTime * characterRigidbody.angularDrag) - characterRigidbody.angularVelocity, ForceMode2D.Impulse);
    }

    public void Dash(Vector2 inputAxis)
    {
        if (dashCoolDownElapsed <= 0 && dashTimerElapsed <= 0)
        {
            appliedForce = Vector2.zero;

            if (inputAxis.sqrMagnitude > 0)
            {
                dashDirection.x = inputAxis.x;
                dashDirection.y = inputAxis.y;
                dashDirection.Normalize();
            }
            else
            {
                dashDirection = transform.up;
            }

            dashSpeed = (characterBase.DashDistance * characterAttributes.DashDistanceMultiplier) / (characterBase.DashTime * characterAttributes.DashTimeMultiplier);
            dashTimerElapsed = characterBase.DashTime * characterAttributes.DashTimeMultiplier;
            dashCoolDownElapsed = characterBase.DashTime * characterAttributes.DashTimeMultiplier + characterBase.DashCoolDown * characterAttributes.DashCoolDownMultiplier;
        }
    }

    public void AddForce(Vector2 force)
    {
        float mass = characterBase.BodyMass * characterAttributes.BodyMassMultiplier;
        if (mass <= 0f) mass = 0.01f;
        appliedForce += force / mass;
    }

    private void ApplyDrag()
    {
        if (appliedForce != Vector2.zero)
        {
            Vector2 forceBeforeDrag = appliedForce;
            appliedForce -= characterBase.LinearDrag * characterAttributes.LinearDragMultiplier * Time.deltaTime * appliedForce.normalized;
            if (Vector2.Dot(forceBeforeDrag, appliedForce) <= 0) appliedForce = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            isDashing = false;
            dashTimerElapsed = 0;
            AddForce(collision.contacts[0].normal * 10f);
        }
    }


	public void SetRigidbodyMass(float value){
		characterRigidbody.mass = value;
	}
}
