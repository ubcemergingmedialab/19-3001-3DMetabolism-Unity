using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompoundControllerCamera : MonoBehaviour
{
    private static CompoundControllerCamera _instance;
    public static CompoundControllerCamera Instance
    {
        get { return _instance; }
    }

    bool _isDragging = false;


    private Camera camera;
    private Vector3 _startPosition;
    private float _startOrthographicSize;
    private RectTransform rectTransform;

    public ScrollRect scrollView;
    private bool isZooming = false;
    private float dragSpeed = 5f;
    private float zoomSpeed = 50f;
    private float minOrthographicSize = 30f;
    private float maxOrthographicSize = 200f;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        DontDestroyOnLoad(this);

        camera = GetComponent<Camera>();
        rectTransform = GetComponent<RectTransform>();
        _startPosition = GetComponent<RectTransform>().anchoredPosition;
        _startOrthographicSize = GetComponent<Camera>().orthographicSize;
    }

    private void LateUpdate()
    {
        if (_isDragging)
            Pan();
    }


    public void ResetCameraPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = _startPosition;
        camera.orthographicSize = _startOrthographicSize;
        _isDragging = false;

    }

    public void Zoom(float axis)
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scroll * zoomSpeed;

        float newSize = Mathf.Clamp(camera.orthographicSize - zoomAmount, minOrthographicSize, maxOrthographicSize);
        camera.orthographicSize = newSize;
        
        //isZooming = false;
    }

    public void Pan()
    {
        float scrollX = Input.GetAxis("Mouse X") * dragSpeed;
        float scrollY = Input.GetAxis("Mouse Y") * dragSpeed;

        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.x -= scrollX;
        anchoredPosition.y -= scrollY;
        rectTransform.anchoredPosition = anchoredPosition;

    }

    public void ToggleDrag(bool drag)
    {
        _isDragging = drag;

    }

    public void StartZooming()
    {
        scrollView.enabled = false;
    }

    public void StopZooming()
    {
        scrollView.enabled = true;

    }

}
