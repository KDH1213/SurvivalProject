using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHPBar : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }
}
