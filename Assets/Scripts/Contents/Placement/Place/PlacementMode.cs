using UnityEngine;

public enum PlaceMode
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
    private PlacementSystem system;

    public PlaceMode CurrentMode { get; private set; }

    private void Awake()
    {
        uiController = GetComponent<PlacementUIController>();
        system = GetComponent<PlacementSystem>();
    }
    public void OnInPlacementMode()
    {
        CurrentMode = PlaceMode.Place;
        var goes = GameObject.FindGameObjectsWithTag("CellIndicator");
        foreach (var go in goes)
        {
            go.GetComponent<MeshRenderer>().enabled = true;
        }
        gridVisualization.SetActive(true);
        system.SelectedObject = null;
        uiController.ShowObjectList();
    }

    public void OnOutPlacementMode()
    {
        CurrentMode = PlaceMode.None;
        var goes = GameObject.FindGameObjectsWithTag("CellIndicator");
        foreach (var go in goes)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
        }
        system.SelectedObject = null;
        gridVisualization.SetActive(false);
        uiController.StopShowObjectList();
    }
    public void OnInSelectMode()
    {
        CurrentMode = PlaceMode.Edit;
        gridVisualization.SetActive(true);
        uiController.StopShowObjectList();
        system.SelectedObject = null;
    }

}
