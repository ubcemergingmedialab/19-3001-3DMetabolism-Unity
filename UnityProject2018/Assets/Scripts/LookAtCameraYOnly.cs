using UnityEngine;
using System.Collections;

/// <summary>
/// Making the object this component is attached to face the camera at all times (y direction)
/// </summary>

[ExecuteAlways]
public class LookAtCameraYOnly : MonoBehaviour
{

    public Camera cameraToLookAt;

    /// <summary>
    /// Set camera 
    /// </summary>
    void Start()
    {
        //transform.Rotate( 180,0,0 );
        if (cameraToLookAt == null)
        {
            cameraToLookAt = Camera.main;
        }
    }

    /// <summary>
    /// Set camera 
    /// </summary>
    private void OnEnable()
    {
        //transform.Rotate( 180,0,0 );
        if (cameraToLookAt == null)
        {
            cameraToLookAt = Camera.main;
        }
    }

    /// <summary>
    /// calculate the diffrence of the camera and transform and make it look at the camera is the y direction only
    /// by rotating the transform
    /// </summary>
    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}