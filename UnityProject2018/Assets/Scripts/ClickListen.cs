using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// movements based on click feedbacks and highlighting 
/// Camera move movements using coroutines, including the aggregate zoom function for highlighted pathways
/// Instead of moving the camera, we move the center of the network to simulate camera movement
/// Bounds encapsulation
/// </summary>
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

    /// <summary>
    /// Intilize the coroutine counters and set the network
    /// </summary>
    void Start()
    {
        timeCounter = totalTime;
        time2Counter = totalTime;
        if(!GameObject.Find("Center/Network"))
            Debug.Log("ClickListen.cs Error: Network Parent could not be found.");
        else
            network = GameObject.Find("Center/Network");
    }

    /// <summary>
    /// By holding  the left shift button and clicking onto the object, the camera focus changes onto that object
    /// moves the focus my using MoveRoutine
    /// </summary>
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

    /// <summary>
    /// coroutine 
    /// moves the network chunck by chunk until it has to stop moving.
    /// instead of moving the camera, we move the center of the network to change focus
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns></returns>
    private IEnumerator MoveRoutine(Vector3 chunk)
    {
        isMoving = true;

        while (isMoving)
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

    /// <summary>
    /// cretes a coroutine to smoothly move on to the new aggreated zoom (highlighted pathway) by changing the camera distance from the focus
    /// </summary>
    /// <param name="newDistance"></param>
    /// <returns></returns>
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

   /// <summary>
   /// given a bounds , it moves the center of the bound onto the center of the camera 
   /// mainly used for the aggregate highlighted poathway zoom
   /// </summary>
   /// <param name="bounds"></param>
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

    /// <summary>
    /// similar to CneterCamera but with a collider instead of bounds
    /// </summary>
    /// <param name="collider"></param>
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


    /// <summary>   
    /// encapsulates all the input bounds into one 
    /// used when new pathways are highlighted and need to be in camera
    /// </summary>
    /// <param name="targetBoundsList"></param>
    /// <returns>encapsulated bounds of all the input bounds</returns>
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
