using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Mode
{
    None,
    Place,
    Select
}

public class PlacementMode : MonoBehaviour
{
    [SerializeField] // ��ġ��忡�� ���̴� ������
    private GameObject gridVisualization;
    private PlacementUIController uiController;

    public Mode CurrentMode { get; private set; }

    private void Awake()
    {
        uiController = GetComponent<PlacementUIController>();
    }
    public void OnInPlacementMode()
    {
        // ���� �۾� ��忡 �� �� �ʿ��� ���� �ۼ�
        CurrentMode = Mode.Place;
        gridVisualization.SetActive(true);
        uiController.ShowObjectList();
    }

    public void OnOutPlacementMode()
    {
        // ���� �۾� ��忡�� ���� �� �ʿ��� ���� �ۼ�
        CurrentMode = Mode.None;
        gridVisualization.SetActive(false);
        uiController.StopShowObjectList();
    }
    public void OnInSelectMode()
    {
        CurrentMode = Mode.Select;
    }

}
