using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera; 
    void Start()
    {
        mainCamera = Camera.main;
    }

   /// <summary>
   /// sets the mitochondrian y rotation to the whatever the camera is , in order get a billboarding effect
   /// </summary>
    void LateUpdate()
    {
        Vector3 newRotation = mainCamera.transform.eulerAngles;
        newRotation.x = 0;
        newRotation.z = 0;
        this.transform.eulerAngles = newRotation;
        this.transform.Rotate(0, 180, 0);
    }
}
