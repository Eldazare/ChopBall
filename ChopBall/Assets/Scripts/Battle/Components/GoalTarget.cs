using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTarget : MonoBehaviour {
    
    public float MinVelocity;

    private Collider2D targetCollider;
    //private SpriteRenderer sprite;
	private MeshRenderer meshRenderer;

    private bool active = true;

	private string soundTarget1Path;

    public void Activate()
    {
        active = true;
        targetCollider.enabled = true;
		meshRenderer.enabled = true;
    }

    public void DeActivate()
    {
        active = false;
        targetCollider.enabled = false;
		meshRenderer.enabled = false;
    }

    private void Awake()
    {
        targetCollider = GetComponent<Collider2D>();
		meshRenderer = GetComponentInChildren<MeshRenderer> ();
		soundTarget1Path = SoundPathController.GetPath ("Target1");
    }

	/*
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
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            if (collision.relativeVelocity.magnitude >= MinVelocity)
            {
				DeActivate ();
            }

			FMODUnity.RuntimeManager.PlayOneShot (soundTarget1Path, gameObject.transform.position);
        }
    }
}
