using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    public Transform targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTarget()
    {
        GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeFocus(targetPosition);
    }
}
