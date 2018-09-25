using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private Vector2 velocity;
    private float angularVelocity;

    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private float rotationAnalogMultiplier;

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
        moveDirection = inputAxis;

        if (dashTimerElapsed <= 0)
        {
            velocity.x = moveDirection.x * characterBase.MovementSpeed * characterRigidbody.drag * Time.deltaTime;
            velocity.y = moveDirection.y * characterBase.MovementSpeed * characterRigidbody.drag * Time.deltaTime;
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
        Vector2 rotateInputDirection = inputAxis;

        rotationAnalogMultiplier = rotateInputDirection.magnitude;
        if (rotationAnalogMultiplier > 0)
        {
            lookDirection.x = rotateInputDirection.x;
            lookDirection.y = rotateInputDirection.y;
            lookDirection.Normalize();
        }

        float angle = Vector2.SignedAngle(transform.up, lookDirection);
        float sign = Mathf.Sign(angle);

        float maxAngularVelocity = Mathf.Abs(angle) * characterRigidbody.angularDrag * Time.deltaTime;
        angularVelocity = Mathf.Abs((characterBase.RotationSpeed * rotationAnalogMultiplier * characterRigidbody.angularDrag * Time.deltaTime));

        angularVelocity = Mathf.Clamp(angularVelocity, 0, maxAngularVelocity);
        angularVelocity *= sign;

        characterRigidbody.AddTorque(angularVelocity - characterRigidbody.angularVelocity, ForceMode2D.Impulse);
    }

    public void Dash()
    {
        if (moveDirection.magnitude != 0)
        {
            dashDirection.x = moveDirection.normalized.x;
            dashDirection.y = moveDirection.normalized.y;
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
