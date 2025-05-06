using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffView : MonoBehaviour
{
    [SerializeField]
    private GameObject debuffSlotPrefab;

    [SerializeField]
    private Transform creatPoint;

    private void Awake()
    {
        var penaltyController = GetComponent<PenaltyController>();

        // penaltyController.
    }
}
