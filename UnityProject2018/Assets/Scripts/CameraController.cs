using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraController: MonoBehaviour
{
    public float movementSpeed = 50f; // Speed of camera movement
    public float padding = 1f; // Additional padding around the bounding box
    private float _totalTime = 1f;

    private IEnumerator moveCameraRoutine;

    private bool _autoCameraLock = false;
    public Collider defaultCenter;

    // moves the camera onto the the target bounds
    public void MoveCameraToHighlightedTarget()
    {
        if (_autoCameraLock) { return; } // if the camera like is true, automatic camera movement is disabled
        if (moveCameraRoutine != null)
        {
            StopCoroutine(moveCameraRoutine);
        }
        Bounds targetBounds = GetHighlightedAggregateBounds();
        Vector3 targetPosition = CalculateTargetPosition(targetBounds,1.1f);
        moveCameraRoutine = MoveCameraCoroutine(targetPosition);
        StartCoroutine(moveCameraRoutine);
        Camera.main.GetComponent<MouseOrbit>().ChangeTargetBounds(targetBounds); // set the rotating axis of the camera around the target
    }
    
    /// <summary>
    /// specifically made for nodes and edges where the parent has a mesh renderer. it moves the camera on to the element
    /// </summary>
    /// <param name="targetTransform"></param>
    public void MoveCameraToParentElement(Transform targetTransform)
    {
        if (_autoCameraLock) { return; }
        if (moveCameraRoutine != null)
        {
            StopCoroutine(moveCameraRoutine);
        }
        StopAllCoroutines();
        Renderer ElementRenderer = targetTransform.parent.GetComponent<MeshRenderer>();
        if(ElementRenderer == null)
        {
            Debug.Log("there is no mesh connected to the parent of this component");
            return;
        }
        Vector3 targetPosition = CalculateTargetPosition(ElementRenderer.bounds, 10f); // larger margin
        moveCameraRoutine = MoveCameraCoroutine(targetPosition);
        StartCoroutine(moveCameraRoutine);
        Camera.main.GetComponent<MouseOrbit>().ChangeTargetBounds(ElementRenderer.bounds);

    }

    /// <summary>
    /// move the camera on the given pathway
    /// </summary>
    /// <param name="pathway"></param>
    public void MoveCameraToPathway(PathwaySO pathway)
    {
        if (_autoCameraLock) { return; } // if the camera like is true, automatic camera movement is disabled
        if (moveCameraRoutine != null)
        {
            StopCoroutine(moveCameraRoutine);
        }
        Bounds targetBounds = GetPathwayAggregateBounds(pathway);


        Vector3 targetPosition = CalculateTargetPosition(targetBounds, 1.1f);
        moveCameraRoutine = MoveCameraCoroutine(targetPosition);
        StartCoroutine(moveCameraRoutine);
        Camera.main.GetComponent<MouseOrbit>().ChangeTargetBounds(targetBounds); // set the rotating axis of the camera around the target
    }

    // coroutine that Lerps the camera onto the target position
    private System.Collections.IEnumerator MoveCameraCoroutine(Vector3 targetPosition)
    {
        
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);

        // Debug.Log("Camera moving from: " + startPosition + " to position: " + targetPosition);

        while (transform.position != targetPosition)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float journeyFraction = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, journeyFraction);

            yield return null; 
        }

    }
    /// <summary>
    /// Calculates the Target position of the camera based on the bounds given , in order to fit the bounds inside the cameraFOV
    /// </summary>
    /// <param name="targetBounds"> the bounds of the thing we want to focus on</param>
    /// <param name="ScreenMargin"> how much padding do we want to add</param>
    /// <returns></returns>
    private Vector3 CalculateTargetPosition(Bounds targetBounds, float ScreenMargin)
    {

        Vector3 boundsCenter = targetBounds.center;
        Vector3 cameraDirection = -Camera.main.transform.forward;//targetBounds.size.normalized;

        //float margin = 1.1f;
        float distance = (targetBounds.extents.magnitude * ScreenMargin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f); //calcs the camera distance to the corresponding bounds

        Vector3 targetPosition = boundsCenter + cameraDirection * distance ;

        return targetPosition;
    }


    // returns the aggreagate of the bounds of the highlighted pathways given by highlightservice. if no pathway highlighted, it gives the defaultCneter
    private Bounds GetHighlightedAggregateBounds()
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

    /// <summary>
    /// returns the aggregate bounds of all the nodes and edges in the pathway by searching for all the components with their tag
    /// </summary>
    /// <param name="pathway"></param>
    /// <returns></returns>
    private Bounds GetPathwayAggregateBounds(PathwaySO pathway)
    {
        Bounds bounds;

        List<Bounds> boundsList = new List<Bounds>();

        foreach(NodeSO nodeSO in pathway.nodes)
        {
            foreach (GameObject node in GameObject.FindGameObjectsWithTag(nodeSO.name))
            {
                boundsList.Add(node.transform.parent.GetComponent<MeshRenderer>().bounds);
            }
        }
        foreach (EdgeSO edgeSO in pathway.edges)
        {
            foreach (GameObject edge in GameObject.FindGameObjectsWithTag(edgeSO.name))
            {
                boundsList.Add(edge.transform.parent.GetComponent<MeshRenderer>().bounds);
            }
        }

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

    /// <summary>
    /// switches the boolean of the AutoCameraLock. ie locks and unlocks the camera movement
    /// </summary>
    public void SwitchAutoLock()
    {
        _autoCameraLock = (!_autoCameraLock);
    }

    public bool GetAutoLock()
    {
        return _autoCameraLock;
    }
}
