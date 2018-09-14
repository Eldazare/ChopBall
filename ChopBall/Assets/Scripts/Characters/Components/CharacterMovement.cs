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

    private Vector2 direction;
    private Vector2 mousePosition;
    private Vector2 lookDirection;
    private float rotationAnalogMultiplier;

    private Vector2 velocity;
    private float angularVelocity;

    private Vector2 dashDirection;
    private float dashTimerElapsed = 0f;
    private float dashCoolDownElapsed = 0f;

    private Rigidbody2D rb2d;
    private InputModel input;

    public void GetInputModel(InputModel model)
    {
        this.input = model;
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //direction.x = Input.GetAxisRaw("Horizontal");
        //direction.y = Input.GetAxisRaw("Vertical");
        //direction.Normalize();

        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (Input.GetKeyDown(KeyCode.Space) && dashTimerElapsed <= 0f && dashCoolDownElapsed <= 0f)
        //{
        //    Dash();
        //}

        if (dashCoolDownElapsed > 0)
        {
            dashCoolDownElapsed -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (input != null)
        {
            if (input.Dash)
            {
                Dash();
            }

            direction.x = input.XAxisLeft;
            direction.y = input.YAxisLeft;

            // Calculate deadzones manually to be circle shaped --> Move this to input handling!
            // http://www.third-helix.com/2013/04/12/doing-thumbstick-dead-zones-right.html
            float deadzone = 0.19f;
            if (direction.magnitude < deadzone)
                direction = Vector2.zero;
            else
                direction = direction.normalized * ((direction.magnitude - deadzone) / (1 - deadzone));
            // ---------------------------------------------------------------------------------

            if (direction.magnitude > 1f)
            {
                direction.Normalize();
            }

            Vector2 rotateInputDirection = new Vector2(input.XAxisRight, input.YAxisRight);

            if (rotateInputDirection.magnitude < deadzone)
                rotateInputDirection = Vector2.zero;
            else
                rotateInputDirection = rotateInputDirection.normalized * ((rotateInputDirection.magnitude - deadzone) / (1 - deadzone));

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
        }

        if (dashTimerElapsed <= 0)
        {
            velocity.x = direction.x * runSpeed * rb2d.drag * Time.deltaTime;
            velocity.y = direction.y * runSpeed * rb2d.drag * Time.deltaTime;
        }
        else
        {
            velocity.x = dashDirection.x * dashSpeed * rb2d.drag * Time.deltaTime;
            velocity.y = dashDirection.y * dashSpeed * rb2d.drag * Time.deltaTime;

            dashTimerElapsed -= Time.deltaTime;
        }

        //lookDirection = mousePosition - rb2d.position;

        float angle = Vector2.SignedAngle(transform.up, lookDirection);
        float sign = Mathf.Sign(angle);

        float maxAngularVelocity = Mathf.Abs(angle) * rb2d.angularDrag * Time.deltaTime;
        angularVelocity = Mathf.Abs((rotationSpeed * rotationAnalogMultiplier * rb2d.angularDrag * Time.deltaTime));

        angularVelocity = Mathf.Clamp(angularVelocity, 0, maxAngularVelocity);
        angularVelocity *= sign;

        rb2d.AddForce(velocity - rb2d.velocity, ForceMode2D.Impulse);
        rb2d.AddTorque(angularVelocity - rb2d.angularVelocity, ForceMode2D.Impulse);
    }

    private void Dash()
    {
        if (direction.magnitude != 0)
        {
            dashDirection.x = direction.normalized.x;
            dashDirection.y = direction.normalized.y;
        }
        else
        {
            dashDirection = transform.up;
        }

        dashTimerElapsed = dashTime;
        dashCoolDownElapsed = dashTime + dashCoolDown;
    }
}
