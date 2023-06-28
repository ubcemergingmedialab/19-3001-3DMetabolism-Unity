using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using static UnityEditor.Experimental.GraphView.GraphView;
/// <summary>
/// This is attached to the Compound Controller Image on the side card.
/// The name suggests it's just for controlling rotation and dragging, 
/// but this class also controls the zoom, panning and other functionality needed for the Compound.
/// </summary>
public class DragToRotate : MonoBehaviour
{
    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    bool isDragging = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (IsPointerOverUIElement())
            if (IsPointerOverCompoundControllerImage(GetEventSystemRaycastResults()))
            {
                if (MouseOrbit.Instance.IsOrbiting())
                {
                    MouseOrbit.Instance.SetIsOrbiting(false);
                }
                
                MouseOrbit.Instance.SetCanOrbit(false);
                isDragging = true;

                if (GetComponent<CompoundController>())
                {
                    GetComponent<CompoundController>().parentScrollView.vertical = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                MouseOrbit.Instance.SetCanOrbit(true);
                isDragging = false;

                if (GetComponent<CompoundController>())
                {
                    GetComponent<CompoundController>().parentScrollView.vertical = true;
                }
            }
        }
        // Checking for scroll wheel push. This will mimic the controls for the big model, and allow user to move the compound in the side card.
        else if (Input.GetMouseButtonDown(2))
        {
            if (IsPointerOverCompoundControllerImage(GetEventSystemRaycastResults()))
            {
                CompoundControllerCamera.Instance.ToggleDrag(true);
            }
        }
        if (Input.GetMouseButtonUp(2))
        {
            CompoundControllerCamera.Instance.ToggleDrag(false);
        }
        // Zoom camera using mouse scroll wheel
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0 && IsPointerOverCompoundControllerImage(GetEventSystemRaycastResults()))
        {
            CompoundControllerCamera.Instance.Zoom(scrollWheel);    
            
        }

        if (isDragging)
        {
            mPosDelta = Input.mousePosition - mPrevPos;

            float horizontalRotation = -mPosDelta.x;
            float verticalRotation = mPosDelta.y;

            transform.Rotate(Vector3.up, horizontalRotation, Space.World);
            transform.Rotate(Vector3.right, verticalRotation, Space.World);

        }


        mPrevPos = Input.mousePosition;
    }


    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    // Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if mouse is over the compound controller image specifically
    /// </summary>
    /// <param name="raycastResults"></param>
    /// <returns></returns>
    private bool IsPointerOverCompoundControllerImage(List<RaycastResult> raycastResults)
    {
        //"Compound Controller Image"
        for (int index = 0; index < raycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raycastResults[index];
            if (curRaysastResult.gameObject.name == "Compound Controller Image")
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
