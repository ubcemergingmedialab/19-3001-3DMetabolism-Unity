using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System.Linq;

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
    private Bounds targetBounds;
    public GameObject targetInFocus;
    public float distance = 5.0f;
    public float defaultDistance = 13.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float dragSpeed = 10f;
    public float rotationSpeed = 50f;
    public float zoomSpeed = 50f;
    public float distanceMin = .5f;
    public float distanceMax = 35f;

    public float scaleSpeed = 0.1f;
    public float scaleMin = 0.001f;
    public float scaleMax = 5f;

    public float scaleThreshhold = 0.1f;

    private bool isLines = false;

    private Rigidbody rigidbody;
    private Vector3 prevMousePos;
    private Vector3 prevMousePosY;
    private Vector3 prevMousePosX;

    private bool needsUpdate = false;
    private bool _isOrbiting = true;
    private bool _canOrbit = true;
    private bool _clickedOnUIFirst = false;

    private bool _isDragging = false;
    private bool _isRotating = false;

    public bool cameraLocked = false;


    float x = 0.0f;
    float y = 0.0f;

    public CameraLabelController cameraLabelController;

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
        if(target.transform.GetComponent<Renderer>() != null)
        {
            targetBounds = target.transform.GetComponent<Renderer>().bounds; // rotate the camera around this
        }

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }       
    }
        


    void LateUpdate()
    { 
        if (cameraLocked) { return; }

        if (!_canOrbit)
            return;

        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(2))
            if (IsPointerOverNamedUIElements())
            {
                _clickedOnUIFirst = true;
                return;
            }

        // check if the scroll wheel is pressed to drag
        else if (Input.GetMouseButtonDown(2))
        {
            _isDragging = true;
            prevMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(2))
        {
            _isDragging = false;
        }

        // Check if left mouse button is held down
        if (Input.GetMouseButtonDown(0))
        {
            _isRotating = true;
            prevMousePosY = Input.mousePosition;
            prevMousePosX = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
        }

        if (_isDragging)
        {
            Vector3 deltaMousePosition = Input.mousePosition - prevMousePos;

            // Calculate camera movement
            Vector3 cameraMovement = new Vector3(-deltaMousePosition.x, -deltaMousePosition.y, 0) * dragSpeed * Time.deltaTime;

            // Apply camera movement to the camera's position
            transform.Translate(cameraMovement, Space.Self);

            // Update previous mouse position
            prevMousePos = Input.mousePosition;
        }

        // Rotate camera while holding left mouse button
        if (_isRotating)
        {
            Vector3 deltaMousePosY = Input.mousePosition - prevMousePosY;
            // Calculate rotation around the X axis and rotate
            float rotationX = -deltaMousePosY.y * rotationSpeed * Time.deltaTime;
            transform.RotateAround(targetBounds.center, transform.right, rotationX);

            Vector3 deltaMousePosX = Input.mousePosition - prevMousePosX;

            // Calculate rotation around the Y axis and rotate
            float rotationY = deltaMousePosX.x * rotationSpeed * Time.deltaTime;
            transform.RotateAround(targetBounds.center, Vector3.up, rotationY);

            // Update previous mouse position
            prevMousePosY = Input.mousePosition;
            prevMousePosX = Input.mousePosition;
        }

        // Zoom camera using mouse scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0 && !IsPointerOverNamedUIElements())
        {
            // Calculate zoom amount
            float zoomAmount = -scrollWheel * zoomSpeed;

            // Calculate new zoom position
            Vector3 zoomPosition = transform.position + transform.forward * zoomAmount;

            // Clamp zoom position within min and max zoom distances
            zoomPosition = Vector3.ClampMagnitude(zoomPosition - transform.position, distanceMax) + transform.position;
            zoomPosition = Vector3.ClampMagnitude(zoomPosition - transform.position, -distanceMin) + transform.position;

            // Apply zoom position to the camera's position
            transform.position = zoomPosition;
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

    public void ChangeTargetBounds(Bounds newTarget)
    {
        targetBounds = newTarget;
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
    public bool IsPointerOverNamedUIElements()
    {
        List<RaycastResult> raycastResults = GetEventSystemRaycastResults();
        //"Compound Controller Image"
        for (int index = 0; index < raycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raycastResults[index];
            if (curRaysastResult.gameObject.name.Contains("Menu") || curRaysastResult.gameObject.name.Contains("EdgeUI") 
                || curRaysastResult.gameObject.name.Contains("PathwayUI") || curRaysastResult.gameObject.name.Contains("NodeUI") 
                || curRaysastResult.gameObject.name.Contains("SearchResultButton") || curRaysastResult.gameObject.name.Contains("SearchContent"))
                return true;
        }
        return false;
    }

    // Gets all event system raycast results of current mouse or touch position.
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

    public bool IsPointerOverUI()
    {
        return IsPointerOverNamedUIElements();
    }

    public void ToggleCameraLock(bool state)
    {
        cameraLocked = state;
    }
}