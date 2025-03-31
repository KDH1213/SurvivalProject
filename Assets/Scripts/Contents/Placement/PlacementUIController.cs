using DG.Tweening;
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

    [SerializeField] // ��ġ / ��� ��ư
    private GameObject placementUI;
    [SerializeField] // ������ư
    private GameObject distroyButton;
    [SerializeField] // �Ǽ��� ��� UI (�׸�) 
    private GameObject Objectcontents;
    [SerializeField] // �Ǽ��� ��� UI
    private GameObject ObjectListUi;

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

    public void ShowObjectList()
    {
        RectTransform rectTran = ObjectListUi.GetComponent<RectTransform>();
        Vector3 uiPos = Camera.main.WorldToScreenPoint(ObjectListUi.transform.position);
        ObjectListUi.transform.DOMoveY(0, 0.3f);
    }
    public void StopShowObjectList()
    {
        RectTransform rectTran = ObjectListUi.GetComponent<RectTransform>();
        Vector3 uiPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -rectTran.offsetMax.y));

        ObjectListUi.transform.DOLocalMoveY(ObjectListUi.transform.localPosition.y - rectTran.offsetMax.y, 0.3f);

    }
}
