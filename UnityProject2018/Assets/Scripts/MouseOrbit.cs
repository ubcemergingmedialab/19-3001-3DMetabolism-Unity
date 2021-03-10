﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour
{

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float scaleSpeed = 0.1f;
    public float scaleMin = 0.001f;
    public float scaleMax = 5f;

    public float scaleThreshhold = 0.1f;

    private bool isLines = false;

    private Rigidbody rigidbody;


    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }


    void LateUpdate()
    {

        if (target && (Input.GetButton("Fire1") || Input.mouseScrollDelta.y != 0))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            Vector3 newScale = target.localScale;
            newScale.x = Mathf.Clamp(newScale.x + (Input.GetAxis("Mouse ScrollWheel") * scaleSpeed), scaleMin, scaleMax);
            newScale.y = newScale.x;
            newScale.z = newScale.x;
            target.localScale = newScale;

            if(newScale.x <= scaleThreshhold && isLines == false)
            {
                isLines = true;
                Debug.Log("rendering as lines");
            } else if(newScale.x > scaleThreshhold && isLines == true)
            {
                isLines = false;
                Debug.Log("rendering as shapes");
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}