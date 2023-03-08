using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform lookTowards;
    [SerializeField] Vector3 offset;
    [SerializeField] bool isTopDown = true;

    [Header("Camera")]
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.WorldToScreenPoint(targetTransform.position + offset);

        if (transform.position != pos)
        {
            transform.position = pos;
        }

        //Working Method
        //https://stackoverflow.com/questions/55922893/how-to-make-a-2d-arrow-on-the-screen-that-always-point-to-a-3d-object-inside-the
        //Need a math expert to help me understand...or ChatGPT...
        // Get the position of the object in screen space
        Vector3 objScreenPos = Camera.main.WorldToScreenPoint(lookTowards.position);

        // Get the directional vector between your arrow and the object
        Vector3 dir = (objScreenPos - transform.position).normalized;

        // Calculate the angle 
        // We assume the default arrow position at 0° is "up"
        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(dir, Vector3.up));

        // Use the cross product to determine if the angle is clockwise
        // or anticlockwise
        Vector3 cross = Vector3.Cross(dir, Vector3.up);
        angle = -Mathf.Sign(cross.z) * angle;

        // Update the rotation of your arrow
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
    }
}
