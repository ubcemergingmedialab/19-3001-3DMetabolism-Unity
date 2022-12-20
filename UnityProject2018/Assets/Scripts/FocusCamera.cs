using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// component to have the camera focus change onto a different object
/// </summary>
public class FocusCamera : MonoBehaviour
{
    public Transform targetPosition;

/// <summary>
/// moves thecamera focus onto a new gameobject by calling changeFocus in MouseOrbit
/// </summary>
/// <param name="focus"></param>
    public void MoveToTarget(GameObject focus)
    {
        GameObject.Find("MainCamera").GetComponent<MouseOrbit>().ChangeFocus(focus.transform);
    }
}
