using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    [SerializeField]
    private float runSpeed = 8f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float dashSpeed = 16f;
    [SerializeField]
    private float dashTime = 1f;
    [SerializeField]
    private float dashCoolDown = 2f;

    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private float rotationAnalogMultiplier;

    private Vector2 velocity;
    private float angularVelocity;

    private Vector2 dashDirection;
    private float dashTimerElapsed = 0f;
    private float dashCoolDownElapsed = 0f;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (dashTimerElapsed <= 0)
        {
            velocity.x = moveDirection.x * runSpeed * rb2d.drag * Time.deltaTime;
            velocity.y = moveDirection.y * runSpeed * rb2d.drag * Time.deltaTime;
        }
        else
        {
            velocity.x = dashDirection.x * dashSpeed * rb2d.drag * Time.deltaTime;
            velocity.y = dashDirection.y * dashSpeed * rb2d.drag * Time.deltaTime;

            dashTimerElapsed -= Time.deltaTime;
        }

        rb2d.AddForce(velocity - rb2d.velocity, ForceMode2D.Impulse);
    }

    public void Rotate(Vector2 inputAxis)
    {
        Vector2 rotateInputDirection = inputAxis;

        if (rotateInputDirection.magnitude > 1f)
        {
            rotateInputDirection.Normalize();
        }

        rotationAnalogMultiplier = rotateInputDirection.magnitude;
        if (rotationAnalogMultiplier > 0)
        {
            lookDirection.x = rotateInputDirection.x;
            lookDirection.y = rotateInputDirection.y;
            lookDirection.Normalize();
        }

        float angle = Vector2.SignedAngle(transform.up, lookDirection);
        float sign = Mathf.Sign(angle);

        float maxAngularVelocity = Mathf.Abs(angle) * rb2d.angularDrag * Time.deltaTime;
        angularVelocity = Mathf.Abs((rotationSpeed * rotationAnalogMultiplier * rb2d.angularDrag * Time.deltaTime));

        angularVelocity = Mathf.Clamp(angularVelocity, 0, maxAngularVelocity);
        angularVelocity *= sign;

        rb2d.AddForce(velocity - rb2d.velocity, ForceMode2D.Impulse);
        rb2d.AddTorque(angularVelocity - rb2d.angularVelocity, ForceMode2D.Impulse);
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

        dashTimerElapsed = dashTime;
        dashCoolDownElapsed = dashTime + dashCoolDown;
    }
}
