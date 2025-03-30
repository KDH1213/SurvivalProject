using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlacementUIController : MonoBehaviour
{
    private PlacementSystem placementSystem;
    [SerializeField]
    private PlacementPreview preview;

    [SerializeField]
    private GameObject placementUI;
    [SerializeField]
    private GameObject distroyButton;
    [SerializeField]
    private GameObject Objectcontents;

    private void Awake()
    {
        placementSystem = GetComponent<PlacementSystem>();
    }

    private void Update()
    {
        // 프리뷰 시 UI 오브젝트 위치로 옮기기
        if (preview.IsPreview)
        {
            placementUI.transform.position = Camera.main.
                WorldToScreenPoint(preview.PreviewObject.transform.GetChild(0).position);
        }
    }

    public void SetPlaceUI(bool isActive)
    {
        placementUI.SetActive(isActive);
    }

    public void SetDestoryButton(bool isActive)
    {
        distroyButton.SetActive(isActive);
    }

    // 배치 된 갯수에 따라 오브젝트 리스트  UI변경
    public void OnSetObjectListUi(PlacementObjectList database, int ID, List<PlacementObject> placedGameObjects)
    {
        int index = database.objects.FindIndex(data => data.ID == ID);
        GameObject obj = Objectcontents.transform.GetChild(ID).gameObject;
        int currentCount = placedGameObjects.Where(data => data.PlacementData.ID == ID).Count();
        int maxCount = database.objects[index].MaxBuildCount;
        if (currentCount >= maxCount)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"x{maxCount - currentCount}";
    }
}
