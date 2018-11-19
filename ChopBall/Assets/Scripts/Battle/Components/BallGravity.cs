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
    public ParticleSystem BounceParticles;

    private float velocity;
	private string soundBouncePath;
    [SerializeField]
    private bool grounded = false;

    private ParticleSystem.MainModule bParticleMain;

	private bool paused = false;

    private void Awake()
    {
        bParticleMain = BounceParticles.main;
		soundBouncePath = SoundPathController.GetPath ("BallBounce");
    }

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

	public void Pause(){
		if (paused) {
			paused = false;
		} else {
			paused = true;
		}
	}

    private void Update()
    {
		if (paused) {
			return;
		}
        if (!grounded)
        {
            velocity += Gravity * Time.deltaTime;
            transform.localPosition += velocity * Vector3.forward * Time.deltaTime;

            if (transform.localPosition.z >= 0f)
            {
                ContactGround();
            }
        }
    }

    public void AddUpwardsVelocity(float amount)
    {
        velocity = -LimitVelocity(amount);
        grounded = false;
    }

    private void ContactGround()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
        float bounce = LimitVelocity(velocity * Bounce);
        float maxBounce = Mathf.Sqrt(2f * Gravity * MaxHeight);

        if (bounce > MinBounce)
        {
            //Bounce
            Debug.Log("Bounce");
            velocity = -bounce;
            if (bounce > maxBounce / 3)
            {
				Debug.Log ("MinBounce");
                bParticleMain.startLifetime = (bounce / maxBounce) * 0.5f;
                BounceParticles.Play();
				FMODUnity.RuntimeManager.PlayOneShot (soundBouncePath, transform.position);
            }
        }
        else
        {
            //Grounded
            //Debug.Log("Grounded");
            grounded = true;
            velocity = 0f;
        }
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
