using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

    private bool _isOrbiting = true;
    private bool _canOrbit = true;
    private bool _clickedOnUIFirst = false;
    
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

    void Start()
    {
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

        if (!_canOrbit)
            return;

        if (Input.GetMouseButtonDown(0))
            if (IsPointerOverNamedUIElements())
            {
                _clickedOnUIFirst = true;
                return;
            }

        if (target && (Input.GetButton("Fire1") || Input.mouseScrollDelta.y != 0) && _isOrbiting) // in case a mouse event happens
        {
            if (_clickedOnUIFirst)
                return;

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

                if (distance >= scaleThreshhold && isLines == false)
                {
                    isLines = true;
                }
                else if (distance > scaleThreshhold && isLines == true)
                {
                    isLines = false;
                }
                needsUpdate = false;

            }
        }
        else if (!target && Input.GetButton("Fire1"))
        {
            SetIsOrbiting(true);
        }

        if (needsUpdate)
        {                                              // only when update is needed, update the distance                
            needsUpdate = false;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);
            rotation = Quaternion.Euler(y, x, 0);
            negDistance = new Vector3(0.0f, 0.0f, -distance);
            position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;

            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            if (distance >= scaleThreshhold && isLines == false)
            {
                isLines = true;
            }
            else if (distance > scaleThreshhold && isLines == true)
            {
                isLines = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _clickedOnUIFirst = false;
            SetIsOrbiting(true);
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

    public void SetIsOrbiting(bool boolean)
    {
        _isOrbiting = boolean;
    }

    public void SetCanOrbit(bool canOrbit)
    {
        this._canOrbit = canOrbit;
    }

    public bool IsOrbiting()
    {
        return _isOrbiting;
    }

    public void TestDebugLog()
    {
        Debug.Log("BUtton pressed!");
    }

    /// <summary>
    /// Checks if mouse is over certain UI elements
    /// </summary>
    /// <param name="raycastResults"></param>
    /// <returns></returns>
    private bool IsPointerOverNamedUIElements()
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();
        //"Compound Controller Image"
        for (int index = 0; index < raycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raycastResults[index];
            if (curRaysastResult.gameObject.name.Contains("Menu") || curRaysastResult.gameObject.name.Contains("EdgeUI") || curRaysastResult.gameObject.name.Contains("PathwayUI") || curRaysastResult.gameObject.name.Contains("NodeUI"))
                return true;
        }
        return false;
    }

    // Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}