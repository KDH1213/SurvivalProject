using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlacementUIObjectInfo : MonoBehaviour
{
    public PlacementObjectInfo placementInfo;
    public Image icomImage;
    public PlacementSystem system;
    public int leftCount;
    [SerializeField]
    private Button clickButton;
    private PlacementUIController uiController;
    
    

    private void Awake()
    {
        // clickButton = GetComponent<Button>();
    }

    public void SetUIObjectInfo(PlacementObjectInfo info, PlacementSystem system)
    {
        placementInfo = info;
        this.system = system;
        GetComponentInChildren<TextMeshProUGUI>().text = $"x{placementInfo.MaxBuildCount}";
        leftCount = placementInfo.MaxBuildCount;
        icomImage.sprite = placementInfo.LevelList[0].Icon;
        uiController = system.GetComponent<PlacementUIController>();
        clickButton.onClick.AddListener(() => uiController.OnOpenObjectInfo(placementInfo));
    }

    

}
