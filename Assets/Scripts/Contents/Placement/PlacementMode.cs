using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMode : MonoBehaviour
{
    [SerializeField]
    private GameObject gridVisualization;
    private PlacementSystem placementSystem;

    private void Start()
    {
        placementSystem = GetComponent<PlacementSystem>();
        gridVisualization.SetActive(false);
    }
    public void SetPlacementMode()
    {
        // 이후 작업 모드에 들어갈 때 필요한 세팅 작성
        gridVisualization.SetActive(true);
    }

    public void OutPlacementMode()
    {
        // 이후 작업 모드에서 나올 때 필요한 세팅 작성
        gridVisualization.SetActive(false);
    }
}
