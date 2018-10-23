using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTarget : MonoBehaviour {
    
    public float RespawnTime;
    public float MinVelocity;

    private Collider2D targetCollider;
    private SpriteRenderer sprite;

    private bool active = true;
    private float t = 0f;

    public void Activate()
    {
        active = true;
        targetCollider.enabled = true;
        sprite.enabled = true;
    }

    public void DeActivate()
    {
        active = false;
        targetCollider.enabled = false;
        sprite.enabled = false;
    }

    private void Awake()
    {
        targetCollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (RespawnTime > 0f)
        {
            if (t > 0f) t -= Time.deltaTime;
            else if (!active)
            {
                targetCollider.enabled = true;
                sprite.enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            if (collision.relativeVelocity.magnitude >= MinVelocity)
            {
                targetCollider.enabled = false;
                sprite.enabled = false;
                t = RespawnTime;
            }
        }
    }
}
