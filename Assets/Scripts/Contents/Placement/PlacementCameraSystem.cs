using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using static Cinemachine.CinemachineVirtualCameraBase;
using DG.Tweening;

public class PlacementCameraSystem : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera placementCamera;
    [SerializeField]
    private CinemachineVirtualCamera currentCamera;
    private PlacementInput inputManager;
    [SerializeField]
    private CinemachineBlendListCamera blendList;

    private CinemachineVirtualCameraBase vCam1;
    private CinemachineVirtualCameraBase vCam2;

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

    public bool IsDrag { get; private set; }

    private void Awake()
    {
        inputManager = GetComponent<PlacementInput>();
        blendList.m_Loop = false;

        vCam1 = currentCamera.GetComponent<CinemachineVirtualCameraBase>();
        vCam2 = placementCamera.GetComponent<CinemachineVirtualCameraBase>();

        blendList.m_Instructions[0].m_VirtualCamera = vCam1;
        blendList.m_Instructions[1].m_VirtualCamera = vCam1;

        //blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        //blendList.m_Instructions[1].m_Blend.m_Time = 0.3f;
    }

    // todo : ��ġ �ý��� ȭ�� <- �÷��� ȭ��
    public void InPlacementCamera()
    {
        placementCamera.transform.position = cameraDefaultPosition;
        placementCamera.m_Lens.FieldOfView = cameraDefaultFOV;

        blendList.m_Instructions[0].m_VirtualCamera = vCam1;
        blendList.m_Instructions[1].m_VirtualCamera = vCam2;
    }

    // todo : ��ġ �ý��� ȭ�� -> �÷��� ȭ��
    public void OutPlacementCamera()
    {
        blendList.m_Instructions[0].m_VirtualCamera = vCam2;
        blendList.m_Instructions[1].m_VirtualCamera = vCam1;
    }

    // todo : ��ġ �ý��� ȭ�� �ܾƿ�
    public void OnZoomInAndOut(InputAction.CallbackContext value)
    {
        float axis = value.ReadValue<float>();
        if (axis < 0)
        {   
            if (placementCamera.m_Lens.FieldOfView <= minZoom)
            {
                placementCamera.m_Lens.FieldOfView = minZoom;
                return;
            }
            placementCamera.m_Lens.FieldOfView -= scrollSpeed;
            moveSpeed = Mathf.Clamp(moveSpeed + moveSpeed * (maxZoom - placementCamera.m_Lens.FieldOfView) / maxZoom, 
                10f, 30f);

        }
        else if (axis > 0)
        {
            if (placementCamera.m_Lens.FieldOfView >= maxZoom)
            {
                placementCamera.m_Lens.FieldOfView = maxZoom;
                return;
            }
            placementCamera.m_Lens.FieldOfView += scrollSpeed;
            moveSpeed = Mathf.Clamp(moveSpeed - moveSpeed * (maxZoom - placementCamera.m_Lens.FieldOfView) / maxZoom, 
                10f, 30f);
            
        }
        
    }

    // todo : ��ġ �ý��� ȭ�� ������
    public void MoveCamera(InputAction.CallbackContext value)
    {
        MousePos = value.ReadValue<Vector2>();

        /*Vector2 dir = (startClickPos - mousePos).normalized;
        placementCamera.transform.position += new Vector3(dir.x, 0, dir.y) * 5f;*/
    }

    // todo : ��ġ �ý��� ȭ�� �巡�� �� �۾�
    public void OnDragMouse(InputAction.CallbackContext value)
    {
        if(inputManager.IsPointerOverUi)
        {
            return;
        }

        if (value.performed)
        {
            if (inputManager.IsObjectSelected)
            {
                
            }
            else
            {
                IsDrag = true;
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
            }
        }
        
        
        if (value.canceled)
        {
            startClickPos = MousePos;
            IsDrag = false;
        }
    }

    // todo : ��ġ �ý��� ȭ�� Ŭ�� �� �۾�
    public void OnClick(InputAction.CallbackContext value)
    {
          // ��ũ�� ��ǥ
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
