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
    public Camera PlacementCamera => cameraSys.placementCamera;

    public Vector3 LastPosition { get; private set; }

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClickPlace;

    public bool IsPointerOverUi { get; set;}
        

    public bool IsPointerOverMap { get; private set; }

    private void Update()
    {
        Debug.Log(OnClickPlace);
        if (EventSystem.current.IsPointerOverGameObject())
        {
            IsPointerOverUi = true;
        }
        else
        {
            IsPointerOverUi = false;
        }
    }

    public void SelectedMapPosition(InputAction.CallbackContext value)
    {
        if (value.phase != InputActionPhase.Started)
        {
            return;
        }
        Vector3 mousePos = cameraSys.MousePos;
        mousePos.z = PlacementCamera.nearClipPlane;
        Ray ray = PlacementCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, placementLayermask))
        {
            if(!IsPointerOverUi)
            {
                if(placementLayermask == LayerMask.NameToLayer("Object"))
                {

                }
                LastPosition = hit.point;
                OnClickPlace?.Invoke();
            }
        }
    }
}
