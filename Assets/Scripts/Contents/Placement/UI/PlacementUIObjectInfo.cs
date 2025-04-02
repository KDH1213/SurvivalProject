using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUIObjectInfo : MonoBehaviour
{
    public PlacementObjectInfo placementInfo;
    public Image icomImage;
    public PlacementSystem system;
    public int leftCount;

    private void Awake()
    {
        Button onClickButton = GetComponent<Button>();
        onClickButton.onClick.AddListener(() => system.StartPlacement(placementInfo.ID));
    }

    public void SetUIObjectInfo(PlacementObjectInfo info, PlacementSystem system)
    {
        placementInfo = info;
        this.system = system;
        GetComponentInChildren<TextMeshProUGUI>().text = $"x{placementInfo.MaxBuildCount}";
        icomImage.sprite = placementInfo.LevelList[0].Icon;
    }

}
