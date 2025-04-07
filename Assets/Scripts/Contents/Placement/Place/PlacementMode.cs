using UnityEngine;

public enum Mode
{
    None,
    Place,
    Edit
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
        CurrentMode = Mode.Place;
        gridVisualization.SetActive(true);
        uiController.ShowObjectList();
    }

    public void OnOutPlacementMode()
    {
        CurrentMode = Mode.None;
        gridVisualization.SetActive(false);
        uiController.StopShowObjectList();
    }
    public void OnInSelectMode()
    {
        CurrentMode = Mode.Edit;
        gridVisualization.SetActive(true);
        uiController.StopShowObjectList();
    }

}
