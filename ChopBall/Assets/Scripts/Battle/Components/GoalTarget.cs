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

	private string soundTarget1Path;

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
		soundTarget1Path = SoundPathController.GetPath ("Target1");
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

			FMODUnity.RuntimeManager.PlayOneShot (soundTarget1Path, gameObject.transform.position);
        }
    }
}
