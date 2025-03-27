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

    public Vector3 LastPosition { get; private set; }

    [SerializeField]
    private LayerMask placementLayermask;
    [SerializeField]
    private LayerMask bothLayermask;

    public event Action OnClickPlace;

    public bool IsPointerOverUi { get; set;}
    public bool IsPointerOverMap { get; private set; }
    public bool IsObjectSelected { get; private set; }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsPointerOverUi = true;
        }
        else
        {
            IsPointerOverUi = false;
        }
    }

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
                if (hit.layer == LayerMask.NameToLayer("Object") && placementObject.IsPlaced)
                {
                    IsObjectSelected = true;
                    Debug.Log("Ãæµ¹!");
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
            
            IsObjectSelected = false;
            Debug.Log("Äµ½½!");
        }
    }

    public Collider GetClickHit()
    {
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

        if (Physics.Raycast(ray, out hit, 100f, bothLayermask))
        {
            return hit.collider;
        }
        return null;
    }

    private void CheckPressHit(GameObject hit)
    {
        if (hit.layer == LayerMask.NameToLayer("Object"))
        {
            PlacementObject placementObject = hit.GetComponent<PlacementObject>();
            if (placementObject.IsPlaced && placementSys.SelectStructure(placementObject))
            {

            }
            else
            {
                OnClickPlace?.Invoke();
            }
        }
        else
        {
            OnClickPlace?.Invoke();
        }
    }

    public void SetLastPos(Vector3 pos)
    {
        LastPosition = pos;
    }
}
