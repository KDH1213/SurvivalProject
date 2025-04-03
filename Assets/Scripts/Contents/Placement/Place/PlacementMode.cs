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
    [SerializeField] // 배치모드에서 보이는 격자판
    private GameObject gridVisualization;
    private PlacementUIController uiController;

    public Mode CurrentMode { get; private set; }

    private void Awake()
    {
        uiController = GetComponent<PlacementUIController>();
    }
    public void OnInPlacementMode()
    {
        // 이후 작업 모드에 들어갈 때 필요한 세팅 작성
        CurrentMode = Mode.Place;
        gridVisualization.SetActive(true);
        uiController.ShowObjectList();
    }

    public void OnOutPlacementMode()
    {
        // 이후 작업 모드에서 나올 때 필요한 세팅 작성
        CurrentMode = Mode.None;
        gridVisualization.SetActive(false);
        uiController.StopShowObjectList();
    }
    public void OnInSelectMode()
    {
        CurrentMode = Mode.Select;
    }

}
