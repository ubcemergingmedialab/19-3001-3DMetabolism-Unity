using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListen : MonoBehaviour
{
    public float totalTime = 1f;
    private float timeCounter;
    private float time2Counter;
    public float moveSplit = 100f;
    private float dynamicDistance;
    private IEnumerator moveRoutine;
    private IEnumerator distanceRoutine;
    private GameObject network;
    private bool isMoving = false;
    private bool isZooming = false;
    public bool objectIsTargetFocus = true;
    // Start is called before the first frame update
    void Start()
    {
        timeCounter = totalTime;
        time2Counter = totalTime;
        if(!GameObject.Find("Center/Network"))
            Debug.Log("ClickListen.cs Error: Network Parent could not be found.");
        else
            network = GameObject.Find("Center/Network");
    }

    private void OnMouseOver()
    {
        if(objectIsTargetFocus) {
            bool hasLShiftClicked = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse0);
            if(!isMoving && hasLShiftClicked)
            {
                Vector3 moveChunk = (-1 * transform.position) * totalTime/moveSplit;
                moveRoutine = MoveRoutine(moveChunk);
                StartCoroutine(moveRoutine);
            }
        }
    }

    private IEnumerator MoveRoutine(Vector3 chunk)
    {
        isMoving = true;

        while(isMoving)
        {
            yield return new WaitForSeconds(totalTime / moveSplit);
            network.transform.position += chunk;
            timeCounter -= totalTime / moveSplit;
            if(timeCounter <= 0)
            {
                timeCounter = totalTime;
                isMoving = false;
            }
        }
    }

    // creates a coroutine for a smooth movement onto the new aggregated zoom value for highlighted path ways
    private IEnumerator DistanceRoutine(float newDistance) {

        float startDistance = GameObject.Find("MainCamera").GetComponent<MouseOrbit>().distance;
        float distanceMargin = 0.1f;
        float lerpValue = totalTime / moveSplit;
        isZooming = ((startDistance - distanceMargin) < newDistance && newDistance < (startDistance + distanceMargin))? false : true;
        
        while (isZooming) {

            yield return new WaitForSeconds(totalTime / moveSplit);

            dynamicDistance = Mathf.Lerp(startDistance, newDistance, lerpValue);
            GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(dynamicDistance);

            time2Counter -= totalTime / moveSplit;                                                      // increment time
            lerpValue += totalTime/moveSplit;                                                           // increment Lerp value
            if(time2Counter <= 0)                                                                       
            {
                time2Counter = totalTime;
                isZooming = false; 
                GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(newDistance);
            }
        }
    }
   // takes a bound and moves the center of world to the center of the aggregate view of highlighted Pathways 
    public void CenterCamera(Bounds bounds) {

        if(moveRoutine != null) {                                                   // if a moveRoutine is still running 
            StopCoroutine(moveRoutine);                                             // Stop the current moveRoutine (to avoid conflict)
            timeCounter = totalTime;                                                // reset timer for the next Routine
        }
        if(distanceRoutine != null) {                                               // if a DistanceRoutine is still running 
            StopCoroutine(distanceRoutine);                                         // Stop the current DistanceRoutine (to avoid conflict)
            time2Counter = totalTime;                                               // reset timer for the next Routine
        }
   
        if(bounds != null) {

            float margin = 1.1f;
            float distance = (bounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f); //calcs the camera distance to the corresponding bounds
            Vector3 moveChunk = -1 * bounds.center * totalTime/moveSplit;       
            
            moveRoutine = MoveRoutine(moveChunk);
            StartCoroutine(moveRoutine);                                            // starts moving the network on to a new center

            distanceRoutine = DistanceRoutine(distance);                            // starts moving the camera on a new distance
            StartCoroutine(distanceRoutine);
        }
    }

    // Moves the world's Center on to the collider
    public void ColliderCenterCamera(Collider collider) {
            
        if (collider == null) { throw new ArgumentNullException(nameof(collider));}

        if(moveRoutine != null) {
            StopCoroutine(moveRoutine);
            timeCounter = totalTime;
        }
        if(distanceRoutine != null) {
            StopCoroutine(distanceRoutine);
            time2Counter = totalTime;
        }

        if (collider.bounds != null) {
            float margin = 1.1f;
            float distance = (collider.bounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);

            Vector3 moveChunk = (-1 * collider.bounds.center) * totalTime/moveSplit;

            moveRoutine = MoveRoutine(moveChunk);                               
            distanceRoutine = DistanceRoutine(distance);
            StartCoroutine(moveRoutine);
            StartCoroutine(distanceRoutine);
        }
    }

// takes a list of bounds and ecapsulates all of them into one bound
    public Bounds BoundsEncapsulate(List<Bounds> targetBoundsList){
        Bounds bounds = new Bounds();

        for(int index = 0; index < targetBoundsList.Count; index++) {
            if(index == 0) {
                bounds = targetBoundsList[index];
            } else {
                bounds.Encapsulate(targetBoundsList[index]);                        // encaplsulate all the bounds into one bound
            }
        }
        return bounds;
    }


    // RENDERERS VERSION OF CENTERCAMERA()

    // public void CenterCamera(List<Renderer> targetRenderers) {
    //     Bounds bounds = new Bounds();
    //     for(int index = 0; index < targetRenderers.Count; index++) {
    //         if(index == 0) {
    //             bounds = targetRenderers[index].bounds;
    //         } else {
    //             bounds.Encapsulate(targetRenderers[index].bounds);
    //         }
    //     }
    //     if(bounds != null) {
    //         float margin = 1.1f;
    //         float distance = (bounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);
    //         GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(distance);
    //         Vector3 moveChunk = (-1 * bounds.center) * totalTime/moveSplit;
    //         moveRoutine = MoveRoutine(moveChunk);
    //         StartCoroutine(moveRoutine);
    //     }
   // }
}
