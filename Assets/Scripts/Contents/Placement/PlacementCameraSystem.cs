using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlacementCameraSystem : MonoBehaviour
{
    [SerializeField]
    private Camera placementCamera;
    [SerializeField]
    private Camera currentCamera;

    [SerializeField]
    private Vector3 cameraDefaultPosition;
    [SerializeField]
    private Vector3 cameraDefaultRotation;
    [SerializeField]
    private float cameraDefaultFOV;

    public Vector2 MousePos { get; private set; }

    private Vector2 startClickPos;
    private Vector2 endClickPos;

    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private float scrollSpeed = 2f; 
    [SerializeField]
    private float moveSpeed = 10f;

    private bool isDrag;

    private void Start()
    {
        placementCamera.depth = -1;
        placementCamera.gameObject.SetActive(false);
    }

    public void InPlacementCamera()
    {
        //currentCamera = Camera.current;
        placementCamera.gameObject.SetActive(true);
        float depth = placementCamera.depth;
        placementCamera.transform.position = cameraDefaultPosition;
        placementCamera.transform.rotation = Quaternion.Euler(cameraDefaultRotation);
        placementCamera.fieldOfView = cameraDefaultFOV;

        placementCamera.depth = 10;
        currentCamera.depth = -1;
    }

    public void OutPlacementCamera()
    {
        float depth = placementCamera.depth;
        placementCamera.depth = -1;
        currentCamera.depth = 10;

        placementCamera.transform.position = cameraDefaultPosition;
        placementCamera.gameObject.SetActive(false);
    }

    public void OnZoomInAndOut(InputAction.CallbackContext value)
    {
        float axis = value.ReadValue<float>();
        if (axis < 0)
        {
            if (placementCamera.fieldOfView <= minZoom)
            {
                placementCamera.fieldOfView = minZoom;
                return;
            }
            placementCamera.fieldOfView -= scrollSpeed;
        }
        else if (axis > 0)
        {
            if (placementCamera.fieldOfView >= maxZoom)
            {
                placementCamera.fieldOfView = maxZoom;
                return;
            }
            placementCamera.fieldOfView += scrollSpeed;
        }
    }

    public void MoveCamera(InputAction.CallbackContext value)
    {
        MousePos = value.ReadValue<Vector2>();

        /*Vector2 dir = (startClickPos - mousePos).normalized;
        placementCamera.transform.position += new Vector3(dir.x, 0, dir.y) * 5f;*/
    }

    public void OnDragMouse(InputAction.CallbackContext value)
    {
        Vector2 dir = (startClickPos - MousePos).normalized;
        if (Mathf.Abs(dir.x) < 0.2f)
        {
            dir.x = 0;
        }
        if (Mathf.Abs(dir.y) < 0.2f)
        {
            dir.y = 0;
        }
        
        placementCamera.transform.position += new Vector3(dir.x, 0, dir.y) * moveSpeed * Time.deltaTime;
        if (value.canceled)
        {
            startClickPos = MousePos;
        }
    }

    public void OnClick(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            startClickPos = MousePos;
        }
        else if (value.canceled)
        {
            endClickPos = MousePos;
        }
    }
}
