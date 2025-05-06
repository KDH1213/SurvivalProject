using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationObject : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3 rotationDireciton;


    private void Update()
    {
        var rotationSpeed = rotationDireciton * (speed * Time.deltaTime);
        target.rotation *= Quaternion.Euler(rotationSpeed);
    }
}
