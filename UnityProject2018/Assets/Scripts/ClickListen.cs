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
        // System.Random random = new System.Random();
        // int tag = random.Next(0,50);
        // Debug.Log( "<MV> tag : " + tag + "; Move Routine START!");

        while(isMoving)
        {
            yield return new WaitForSeconds(totalTime / moveSplit);
            // Debug.Log( "<MV> tag : " + tag + "; time Counter: " + timeCounter);
            network.transform.position += chunk;
            timeCounter -= totalTime / moveSplit;
            if(timeCounter <= 0)
            {
                timeCounter = totalTime;
                isMoving = false;
            }
        
        }
        //Debug.Log( "Bounds, Center: " + bounds.center + " extents magnitude: " + bounds.extents.magnitude);
        // Debug.Log( "<MV> tag : " + tag + "; Move Routine DONE!");
    }

    private IEnumerator DistanceRoutine(float newDistance) {
        isZooming = true;

        while (isZooming) {

            yield return new WaitForSeconds(totalTime / moveSplit);
            float currDistance = GameObject.Find("MainCamera").GetComponent<MouseOrbit>().distance;
            dynamicDistance = Mathf.Lerp(currDistance, newDistance, totalTime / moveSplit);
            //Debug.Log( "<D> lerping dynamicDistance : " + dynamicDistance);
            GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(dynamicDistance);

            time2Counter -= totalTime / moveSplit;
            if(time2Counter <= 0)
            {
                time2Counter = totalTime; 
                dynamicDistance = newDistance;
                isZooming = false;
            }

        }
        Debug.Log("Bounds, camera distance: " + GameObject.Find("MainCamera").GetComponent<MouseOrbit>().distance);
       
    }
   // takes a list of Bounds moves the center of world to the center of the aggregate view of highlighted Pathways 
    public void CenterCamera(List<Bounds> targetBoundsList) {

        if(moveRoutine != null) {                                                   // if a moveRoutine is still running 
            //Debug.Log( "<MV> CenterCamera Stoping a Move Routine");
            StopCoroutine(moveRoutine);                                             // Stop the current moveRoutine (to avoid conflict)
            //Debug.Log( "<MV> CenterCamera refreshed timeCounter");
            timeCounter = totalTime;                                                // reset timer for the next Routine
        }
        if(distanceRoutine != null) {                                               // if a DistanceRoutine is still running 
            //Debug.Log( "<D> CenterCamera Stoping Distance Routine");
            StopCoroutine(distanceRoutine);                                         // Stop the current DistanceRoutine (to avoid conflict)
            time2Counter = totalTime;                                               // reset timer for the next Routine
        }

        Bounds bounds = new Bounds();
        for(int index = 0; index < targetBoundsList.Count; index++) {
            if(index == 0) {
                bounds = targetBoundsList[index];
            } else {
                bounds.Encapsulate(targetBoundsList[index]);                        // encaplsulate all the bounds into one bound
            }
        }
            
        if(bounds != null) {
            Debug.Log( "Bounds, Center: " + bounds.center + " extents magnitude: " + bounds.extents.magnitude);

            float margin = 1.1f;
            float distance = (bounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f); //calcs the camera distance to the corresponding bounds

            Vector3 moveChunk = -1 * bounds.center * totalTime/moveSplit;       
            
            //Debug.Log( "<MV> CenterCamera Starting new MoveRoutine");
            moveRoutine = MoveRoutine(moveChunk);
            StartCoroutine(moveRoutine);                                            // starts moving the network on to a new center

            //Debug.Log( "<D> CenterCamera Starting new DistanceRoutine");
            distanceRoutine = DistanceRoutine(distance);                            // starts moving the camera on a new distance
            StartCoroutine(distanceRoutine);
        }
    }

    // Moves the camera Center on to the collider
    public void ColliderCenterCamera(Collider collider) {
            
        if (collider == null) { throw new ArgumentNullException(nameof(collider));}

        if(moveRoutine != null) {
            //Debug.Log( "<MV> ColliderCenter Stoping a Move Routine");
            StopCoroutine(moveRoutine);
            //Debug.Log( "<MV> ColliderCenter refreshed timeCounter");
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
