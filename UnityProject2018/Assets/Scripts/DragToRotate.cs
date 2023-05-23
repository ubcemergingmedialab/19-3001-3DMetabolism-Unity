using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

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
