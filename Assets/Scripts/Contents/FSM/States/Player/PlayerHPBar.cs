using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    private void Update()
    {
        transform.position = playerTransform.position;
    }
}
