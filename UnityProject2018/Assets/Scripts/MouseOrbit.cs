using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour
{
    //SINGLETON
    private static MouseOrbit _instance;
    public static MouseOrbit Instance
    {
        get { return _instance; }
    }
    public Transform target;
    public float distance = 5.0f;
    public float defaultDistance = 13.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float scaleSpeed = 0.1f;
    public float scaleMin = 0.001f;
    public float scaleMax = 5f;

    public float scaleThreshhold = 0.1f;

    private bool isLines = false;

    private Rigidbody rigidbody;
    private bool needsUpdate = false;

    private bool disableOrbit = false; 

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Awake()
    {   
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }


    void LateUpdate()
    {
        Quaternion rotation;
        Vector3 negDistance;
        Vector3 position;

        if (target && (Input.GetButton("Fire1") || Input.mouseScrollDelta.y != 0) && !disableOrbit) // in case a mouse event happens
        {
            if (target && (Input.GetButton("Fire1") || Input.mouseScrollDelta.y != 0) || needsUpdate)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance, distanceMin, distanceMax);

                distance -= Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
                negDistance = new Vector3(0.0f, 0.0f, -distance);
                position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;

                if(distance >= scaleThreshhold && isLines == false)
                {
                    isLines = true;
                } else if(distance > scaleThreshhold && isLines == true)
                {
                    isLines = false;
                }
                needsUpdate = false;
            
                }
        }

        if (needsUpdate) {                                              // only when update is needed, update the distance                
            needsUpdate = false;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);
            rotation = Quaternion.Euler(y, x, 0);
            negDistance = new Vector3(0.0f, 0.0f, -distance);
            position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            if(distance >= scaleThreshhold && isLines == false)
            {
                isLines = true;
            } else if(distance > scaleThreshhold && isLines == true)
            {
                isLines = false;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void ChangeFocus(Transform newTarget)
    {
        
        target = newTarget;
        distance = defaultDistance;
        needsUpdate = true;
    }

    public void ChangeDistance(float dist)
    {
        distance = dist;
        needsUpdate = true;
    }

    public void SetDisableOrbit(bool boolean)
    {
        disableOrbit = boolean;
    }

    public void TestDebugLog () {
        Debug.Log("BUtton pressed!"); }
}