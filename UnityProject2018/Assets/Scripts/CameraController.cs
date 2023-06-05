using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraController: MonoBehaviour
{
    public float movementSpeed = 10f; // Speed of camera movement
    public float padding = 1f; // Additional padding around the bounding box
    public float totalTime = 1f;
    public float moveSplit = 100f;
    private bool AutoCameraLock = false;
    public Collider defaultCenter;

    // moves the camera onto the the target bounds
    public void MoveCameraToTarget()
    {
        if (AutoCameraLock) { return; }
      
        Vector3 targetPosition = CalculateTargetPosition(GetAggregateBounds());
        StopAllCoroutines();
        StartCoroutine(MoveCameraCoroutine(targetPosition));
    }

    // get the aggreagate of the bounds given by the highlightservice
    private Bounds GetAggregateBounds()
    {
        Bounds bounds;
      
        List<Bounds> boundsList = HighlightService.Instance.GetHighlightedBounds();
        if (boundsList.Count == 0)
        {
            Debug.Log(" no PW highlighted defaulting to defaultCenter");
            bounds = defaultCenter.bounds;
        }
        else
        {
         bounds = BoundsEncapsulate(boundsList);
        }
    
        return bounds;
    }

    // coroutine to move the camera onto the new focus
    private System.Collections.IEnumerator MoveCameraCoroutine(Vector3 targetPosition)
    {
        
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);

        while (transform.position != targetPosition)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float journeyFraction = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, journeyFraction);
            yield return null;
        }
    }

    private Vector3 CalculateTargetPosition(Bounds targetBounds)
    {

        Vector3 boundsCenter = targetBounds.center;
        Vector3 cameraDirection = -targetBounds.size.normalized;

        float margin = 1.1f;
        float distance = (targetBounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f); //calcs the camera distance to the corresponding bounds
        Vector3 moveChunk = -1 * boundsCenter * totalTime / moveSplit;


        Vector3 targetPosition = boundsCenter + (cameraDirection * (distance));
        return targetPosition;
    }



    /// <summary>   
    /// encapsulates all the input bounds into one 
    /// used when new pathways are highlighted and need to be in camera
    /// </summary>
    /// <param name="targetBoundsList"></param>
    /// <returns>encapsulated bounds of all the input bounds</returns>
    private Bounds BoundsEncapsulate(List<Bounds> targetBoundsList)
    {
        Bounds bounds = new Bounds();

        for (int index = 0; index < targetBoundsList.Count; index++)
        {
            if (index == 0)
            {
                bounds = targetBoundsList[index];
            }
            else
            {
                bounds.Encapsulate(targetBoundsList[index]);                        // encaplsulate all the bounds into one bound
            }
        }
        return bounds;
    }
}
