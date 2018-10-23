using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    internal bool isDashing = false;

    private Vector2 velocity;
    private float angularVelocity;

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
    }

    public void Move(Vector2 inputAxis)
    {
        if (dashTimerElapsed <= 0)
        {
            isDashing = false;
            velocity.x = inputAxis.x * characterBase.MovementSpeed * characterAttributes.MovementSpeedMultiplier * characterRigidbody.drag * Time.deltaTime;
            velocity.y = inputAxis.y * characterBase.MovementSpeed * characterAttributes.MovementSpeedMultiplier * characterRigidbody.drag * Time.deltaTime;
        }
        else
        {
            isDashing = true;
            velocity.x = dashDirection.x * dashSpeed * characterRigidbody.drag * Time.deltaTime;
            velocity.y = dashDirection.y * dashSpeed * characterRigidbody.drag * Time.deltaTime;

            dashTimerElapsed -= Time.deltaTime;
        }

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
}
