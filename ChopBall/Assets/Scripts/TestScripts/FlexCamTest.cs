using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexCamTest : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    public float smooth = .5f;
    public float minZoom = 60f;
    public float maxZoom = 25f;
    public float zoomLimit = 20f;

    private Camera flexCam;
    private Vector3 velocity;
    private IEnumerator coroutine;

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimit);
        flexCam.fieldOfView = Mathf.Lerp(flexCam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smooth);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[1].position);
        }

        return bounds.center;
    }

    void OnEnable()
    {
        flexCam = GetComponent<Camera>();

        this.gameObject.SetActive(true);
        //coroutine = Wait(3.0f);
        //StartCoroutine(coroutine);
    }

    /*private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            this.gameObject.SetActive(false);
        }
    }*/
}
