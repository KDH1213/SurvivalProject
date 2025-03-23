using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlacementInput : MonoBehaviour
{
    [SerializeField]
    private PlacementCameraSystem cameraSys;
    
    [SerializeField]
    private Camera placementCamera;

    public Vector3 LastPosition { get; private set; }

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClickPlace;

    public bool IsPointerOverUi()
        => EventSystem.current.IsPointerOverGameObject();

    public bool IsPointerOverMap { get; private set; }

    public void SelectedMapPosition(InputAction.CallbackContext value)
    {
        if (value.phase != InputActionPhase.Started)
        {
            return;
        }
        Vector3 mousePos = cameraSys.MousePos;
        mousePos.z = placementCamera.nearClipPlane;
        Ray ray = placementCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, placementLayermask))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Placement"))
            {
                LastPosition = hit.point;
                DoPlacement();
                
            }
            else
            {

            }
        }
    }

    private void DoPlacement()
    {
        OnClickPlace?.Invoke();
    }

    private void DoObject()
    {

    }
}
