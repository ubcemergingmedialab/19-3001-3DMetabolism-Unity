using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MakeItButton : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    private GameObject button;
    private float _lastClickTime = 0f;
    private static float _DOUBLE_CLICK_TIME = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseOrbit.Instance.IsPointerOverNamedUIElements())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject && Time.time - _lastClickTime < _DOUBLE_CLICK_TIME) // double click
            {
                // make it so that the camera focus shifts onto the metabolite/edge
                if (!Camera.main.GetComponent<CameraController>().GetAutoLock())
                {
                    Camera.main.GetComponent<CameraController>().MoveCameraToParentElement(this.transform);
                }
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                _lastClickTime = Time.time;
                unityEvent.Invoke();
            }
        }
    }

    public void ButtonClicked()
    {
        if (GetComponent<EdgeDataDisplay>())
        {
            GetComponent<EdgeDataDisplay>().UpdateScriptableObject();
        }
    }
}
