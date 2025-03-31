using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMode : MonoBehaviour
{
    [SerializeField] // ��ġ��忡�� ���̴� ������
    private GameObject gridVisualization;
    private PlacementUIController uiController;

    private void Awake()
    {
        uiController = GetComponent<PlacementUIController>();
    }
    public void SetPlacementMode()
    {
        // ���� �۾� ��忡 �� �� �ʿ��� ���� �ۼ�
        gridVisualization.SetActive(true);
        uiController.ShowObjectList();
    }

    public void OutPlacementMode()
    {
        // ���� �۾� ��忡�� ���� �� �ʿ��� ���� �ۼ�
        gridVisualization.SetActive(false);
        uiController.StopShowObjectList();
    }
    
    
}
