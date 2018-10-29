using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGravity : MonoBehaviour {

    public float Gravity = 9.81f;
    [Range(0, 1f)]
    public float Bounce = 1f;
    public float MinBounce = 0.01f;
    public float MaxHeight = 2f;

    private float velocity;

    private void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -MaxHeight);
    }

    private void Update()
    {
        velocity += Gravity * Time.deltaTime;
        transform.localPosition += velocity * Vector3.forward;

        if (transform.localPosition.z >= 0f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
            if (velocity > MinBounce) velocity *= -Bounce;
            else velocity = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space)) AddUpwardsVelocity(2f);
    }

    public void AddUpwardsVelocity(float amount)
    {
        float distanceToCeiling = -MaxHeight - transform.localPosition.z;
        float maxVelocity = Mathf.Sqrt(Mathf.Abs(2f * Gravity * distanceToCeiling));
        velocity = -maxVelocity;
    }
}
