using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListen : MonoBehaviour
{
    public float totalTime = 1f;
    private float timeCounter;
    public float moveSplit = 100f;
    private IEnumerator moveRoutine;
    private GameObject network;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        timeCounter = totalTime;
        if(!GameObject.Find("Center/Network"))
            Debug.Log("Network Parent could not be found");
        else
            network = GameObject.Find("Center/Network");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked on " + transform.name);
        if(!isMoving)
        {
            Debug.Log("calling coroutine");
            Vector3 moveChunk = (-1 * transform.position) * totalTime/moveSplit;
            Debug.Log(moveChunk);
            moveRoutine = MoveRoutine(moveChunk);
            StartCoroutine(moveRoutine);
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
}
