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
    private bool isMoveCoroutineRunning = false;
    private bool isDistanceCoroutineRunning = false;
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
            if(!isMoveCoroutineRunning && hasLShiftClicked)
            {
                Vector3 moveChunk = (-1 * transform.position) * totalTime/moveSplit;
                moveRoutine = MoveRoutine(moveChunk);
                StartCoroutine(moveRoutine);
            }
        }
    }

    private IEnumerator MoveRoutine(Vector3 chunk)
    {
        isMoveCoroutineRunning = true;
        while(isMoveCoroutineRunning)
        {
            yield return new WaitForSeconds(totalTime / moveSplit);
            network.transform.position += chunk;
            timeCounter -= totalTime / moveSplit;
            if(timeCounter <= 0)
            {
                timeCounter = totalTime;
                isMoveCoroutineRunning = false;
            }
        }
    }

    private IEnumerator DistanceRoutine(float newDistance) {
        isDistanceCoroutineRunning = true;
        
        while (isDistanceCoroutineRunning) {

            yield return new WaitForSeconds(totalTime / moveSplit);
            float currDistance = GameObject.Find("MainCamera").GetComponent<MouseOrbit>().distance;
            //currentTime = (time2Counter/moveSplit) * totalTime;
            dynamicDistance = Mathf.Lerp(currDistance, newDistance, totalTime / moveSplit);
            GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(dynamicDistance);

            time2Counter -= totalTime / moveSplit;
            if(time2Counter <= 0)
            {
                time2Counter = totalTime; 
                dynamicDistance = newDistance;
                isDistanceCoroutineRunning = false;
            }
            //Debug.Log("looping in DistanceRoutine");
        }
        
       
    }
   // takes a list of Bounds moves the center of world to the center of the aggregate view of highlighted Pathways 
    public void CenterCamera(List<Bounds> targetBoundsList) {

        if(moveRoutine != null) {
            StopCoroutine(moveRoutine);
        }
        if(distanceRoutine != null) {
            StopCoroutine(distanceRoutine);
        }

        Bounds bounds = new Bounds();
        for(int index = 0; index < targetBoundsList.Count; index++) {
            if(index == 0) {
                bounds = targetBoundsList[index];
            } else {
                bounds.Encapsulate(targetBoundsList[index]);
            }
        }
        if(bounds != null) {
            float margin = 1.1f;
            float distance = (bounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f); //calcs the camera distance to the corresponding bounds

            GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(distance);

            Vector3 moveChunk = -(bounds.center - network.transform.position) * totalTime/moveSplit;
            // starts moving on the new focus
            moveRoutine = MoveRoutine(moveChunk);
            StartCoroutine(moveRoutine);
            // starts zooming on the subject
        //     distanceRoutine = DistanceRoutine(distance);
        //     StartCoroutine(distanceRoutine);
        }// }
    }

        // Moves the camera Center on to the collider
    public void ColliderCenterCamera(Collider collider) {
            
        if (collider == null) { throw new ArgumentNullException(nameof(collider));}

        if (collider.bounds != null) {
            float margin = 1.1f;
            float distance = (collider.bounds.extents.magnitude * margin) / Mathf.Sin(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);

            GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeDistance(distance);

            Vector3 moveChunk = (-1 * collider.bounds.center) * totalTime/moveSplit;
            moveRoutine = MoveRoutine(moveChunk);
            // distanceRoutine = DistanceRoutine(distance);
            StartCoroutine(moveRoutine);
            // StartCoroutine(distanceRoutine);
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
