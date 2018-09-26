using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

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
    }

    public void Move(Vector2 inputAxis)
    {
        if (dashTimerElapsed <= 0)
        {
            velocity.x = inputAxis.x * characterBase.MovementSpeed * characterRigidbody.drag * Time.deltaTime;
            velocity.y = inputAxis.y * characterBase.MovementSpeed * characterRigidbody.drag * Time.deltaTime;
        }
        else
        {
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

        angularVelocity = characterBase.RotationSpeed * rotationAnalogMultiplier;

        angularVelocity = Mathf.Clamp(angularVelocity, 0, Mathf.Abs(angle)) * Mathf.Sign(angle);

        characterRigidbody.AddTorque((angularVelocity * Time.deltaTime * characterRigidbody.angularDrag) - characterRigidbody.angularVelocity, ForceMode2D.Impulse);
    }

    public void Dash(Vector2 inputAxis)
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

        dashSpeed = characterBase.DashDistance / characterBase.DashTime;
        dashTimerElapsed = characterBase.DashTime;
        dashCoolDownElapsed = characterBase.DashTime + characterBase.DashCoolDown;
    }
}
