using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlacementInput : MonoBehaviour
{
    [SerializeField]
    private PlacementCameraSystem cameraSys;
    [SerializeField]
    private PlacementSystem placementSys;

    public Vector3 LastPosition { get; set; }

    [SerializeField]
    private LayerMask placementLayermask;
    [SerializeField]
    private LayerMask bothLayermask;

    public bool IsPointerOverUi { get; set;}
    public bool IsPointerOverMap { get; private set; }
    public bool IsObjectHoldPress { get; private set; }

    private void Update()
    {
        // �Է��� ���� UI ������ �˻�
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsPointerOverUi = true;
        }
        else
        {
            IsPointerOverUi = false;
        }
    }

    // Ŭ�� �Է�
    public void OnInputClick(InputAction.CallbackContext value)
    {
        if (IsPointerOverUi || cameraSys.IsDrag)
        {
            return;
        }
        if (value.performed)
        {
            GameObject hit = GetClickHit()?.gameObject;
            if (hit == null)
            {
                return;
            }

            if (value.interaction is HoldInteraction)
            {
                
                PlacementObject placementObject = hit.GetComponent<PlacementObject>();
                if (hit.layer ==  GetLayer.Object)
                {
                    IsObjectHoldPress = true;
                }
                else
                {

                }
            }

            else if (value.interaction is PressInteraction)  
            {
                CheckPressHit(hit);
            }
            
        }

        if (value.canceled)
        {
            
            IsObjectHoldPress = false;
            Debug.Log("ĵ��!");
        }
    }

    public void UpdatePosition()
    {
        if (IsPointerOverUi)
        {
            return;
        }
        Vector3 mousePos = cameraSys.MousePos;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, placementLayermask))
        {
            LastPosition = hit.point;
            IsPointerOverMap = true;
        }
        else
        {
            IsPointerOverMap = false;
        }
    }

    // Ŭ���� ���� �´��� ��ü ���� �� ������ ��ġ ��ġ ����
    public Collider GetClickHit()
    {
        if(IsPointerOverUi)
        {
            return null;
        }
        Vector3 mousePos = cameraSys.MousePos;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, bothLayermask))
        {
            return hit.collider;
        }
        return null;
    }

    // ���� ��ü�� ��ġ������ �˻�
    private void CheckPressHit(GameObject hit)
    {
       
        if (hit.layer == GetLayer.Object)
        {
            PlacementObject placementObject = hit.GetComponent<PlacementObject>();
            if (placementObject.IsPlaced && placementSys.SelectStructure(placementObject))
            {

            }
            else
            {
                
            }
        }
        else
        {
            
        }
    }

    public void SetLastPos(Vector3 pos)
    {
        LastPosition = pos;
    }
}
