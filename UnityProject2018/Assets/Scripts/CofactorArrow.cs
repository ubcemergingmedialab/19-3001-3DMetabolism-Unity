using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofactorArrow : MonoBehaviour
{
    //public Transform targetObject; // The second object you want to point to
    public Vector3 targetPosition;

    public void InitializeTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        Look();
    }

    void Look()
    {
        if (targetPosition != Vector3.zero)
        {
            // Always face the camera
            //transform.LookAt(Camera.main.transform);

            // Point in the direction of the target object
            //Vector3 directionToTarget = targetObject.position - transform.position;
            Vector3 directionToTarget = targetPosition - transform.position;
            transform.rotation *= Quaternion.FromToRotation(Vector3.up, directionToTarget.normalized);
        }
    }
}
