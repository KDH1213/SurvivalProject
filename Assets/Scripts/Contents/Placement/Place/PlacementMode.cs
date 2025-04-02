using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMode : MonoBehaviour
{
    [SerializeField] // 배치모드에서 보이는 격자판
    private GameObject gridVisualization;
    private PlacementUIController uiController;

    private void Awake()
    {
        uiController = GetComponent<PlacementUIController>();
    }
    public void SetPlacementMode()
    {
        // 이후 작업 모드에 들어갈 때 필요한 세팅 작성
        gridVisualization.SetActive(true);
        uiController.ShowObjectList();
    }

    public void OutPlacementMode()
    {
        // 이후 작업 모드에서 나올 때 필요한 세팅 작성
        gridVisualization.SetActive(false);
        uiController.StopShowObjectList();
    }
    
    
}
