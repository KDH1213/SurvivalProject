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
        // ������ �� UI ������Ʈ ��ġ�� �ű��
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

    // ��ġ �� ������ ���� ������Ʈ ����Ʈ  UI����
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
