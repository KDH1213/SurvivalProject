using TMPro;
using UnityEngine;
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
        if(clickButton == null)
        {
            clickButton = GetComponent<Button>();
        }
    }

    public void SetUIObjectInfo(PlacementObjectInfo info, PlacementSystem system)
    {
        placementInfo = info;
        this.system = system;
        GetComponentInChildren<TextMeshProUGUI>().text = $"x{placementInfo.MaxBuildCount}";
        leftCount = placementInfo.MaxBuildCount;
        icomImage.sprite = placementInfo.Icon;
        uiController = system.GetComponent<PlacementUIController>();
        clickButton.onClick.AddListener(() => uiController.OnOpenBuildInfo(placementInfo));
    }

    

}
