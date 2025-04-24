using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocketController : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [field: SerializeField]
    public Vector3 OffsetPosition { get; private set; }

    [field: SerializeField]
    public Quaternion Rotation { get; private set; }

    [field: SerializeField]
    public Vector3 Scale { get; private set; } = Vector3.one;


    [ContextMenu("SetTransformInfo")]
    public void SetTransformInfo()
    {
        OffsetPosition = transform.localPosition;
        Rotation = transform.localRotation;
        Scale = transform.localScale;

        var weaponSocketController = prefab.GetComponent<WeaponSocketController>();
        weaponSocketController.OffsetPosition = OffsetPosition;
        weaponSocketController.Rotation = Rotation;
        weaponSocketController.Scale = Scale;

        weaponSocketController.transform.localPosition = OffsetPosition;
        weaponSocketController.transform.localRotation = Rotation;
        weaponSocketController.transform.localScale = Scale;
    }

    public void OnInitialized()
    {
        transform.localPosition = OffsetPosition;
        transform.localRotation = Rotation;
        transform.localScale = Scale;
    }
}
