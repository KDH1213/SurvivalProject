using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMode : MonoBehaviour
{
    [SerializeField]
    private GameObject gridVisualization;

    private void Start()
    {
        gridVisualization.SetActive(false);
    }
    public void SetPlacementMode()
    {
        // ���� �۾� ��忡 �� �� �ʿ��� ���� �ۼ�
        gridVisualization.SetActive(true);
    }

    public void OutPlacementMode()
    {
        // ���� �۾� ��忡�� ���� �� �ʿ��� ���� �ۼ�
        gridVisualization.SetActive(false);
    }
}
