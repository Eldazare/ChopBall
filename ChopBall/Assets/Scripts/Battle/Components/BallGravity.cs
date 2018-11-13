using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGravity : MonoBehaviour {

    public float Gravity = 9.81f;
    [Range(0, 1f)]
    public float Bounce = 1f;
    public float MinBounce = 0.01f;
    public float StartHeight = 2f;
    public float MaxHeight = 2f;

    private float velocity;

    private void OnEnable()
    {
        SetHeight(StartHeight);
    }

    private void OnValidate()
    {
        SetHeight(StartHeight);
    }

    public void SetHeight(float height)
    {
        transform.localPosition = new Vector3(0, 0, -height);
    }

    private void Update()
    {
        velocity += Gravity * Time.deltaTime;
        transform.localPosition += velocity * Vector3.forward * Time.deltaTime;

        if (transform.localPosition.z >= 0f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
            if (velocity > MinBounce) velocity = -LimitVelocity(velocity * Bounce);
            else velocity = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space)) AddUpwardsVelocity(8f);
    }

    public void AddUpwardsVelocity(float amount)
    {
        velocity = -LimitVelocity(amount);
    }

    private float LimitVelocity(float velocity)
    {
        float distanceToCeiling = MaxHeight + transform.localPosition.z;
        float maxVelocity = (distanceToCeiling > 0f && distanceToCeiling <= MaxHeight) ? Mathf.Sqrt(2f * Gravity * distanceToCeiling) : 0f;
        return Mathf.Min(maxVelocity, velocity);
    }
	/*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.localScale.z / 2 + MaxHeight) * -transform.forward, transform.localScale.z / 2);
        Gizmos.DrawWireSphere(transform.localScale.z / 2 * -transform.forward, transform.localScale.z / 2);
    }
    */
}
