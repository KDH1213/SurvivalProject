using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUIController : MonoBehaviour
{
    private PlacementSystem placementSystem;
    [SerializeField]
    private PlacementPreview preview;
    [SerializeField]
    private PlacementObjectList dataBase;

    [SerializeField] // ��ġ / ��� ��ư
    private GameObject placementUI;
    [SerializeField] // ������ư
    private GameObject distroyButton;
    [SerializeField] // �Ǽ��� ��� UI (�׸�) 
    private List<PlacementUIObjectInfo> Objectcontents = new();
    [SerializeField] // �Ǽ��� ��� UI (�׸�) 
    private GameObject Objectcontent;
    [SerializeField] // �Ǽ��� ��� ������
    private PlacementUIObjectInfo ObjectContentPrefeb;
    [SerializeField] // �Ǽ��� ��� UI
    private GameObject ObjectListUi;
    [SerializeField] // ������ ����Ʈ ��
    private ToggleGroup toggles;
    [SerializeField] // ������ ����Ʈ ��
    private GameObject modeSelectButtons;
    [SerializeField]
    private BuildInfoUI buildInfo; // �Ǽ��� �Ǽ� ���� UI
    [SerializeField]
    private ObjectInfoUI objectInfo; // �Ǽ��� ���� UI
    [SerializeField]
    private UpgradeUI upgradeUI; // �Ǽ��� ���� UI


    private void Awake()
    {
        placementSystem = GetComponent<PlacementSystem>();
    }

    private void Start()
    {
        dataBase = placementSystem.Database;
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

    public void OnShowPlaceUI(bool isActive)
    {
        placementUI.SetActive(isActive);
    }

    // ��ġ �� ������ ���� ������Ʈ ����Ʈ  UI����
    public int OnSetObjectListUi(PlacementObjectList database, int ID, List<PlacementObject> placedGameObjects)
    {
        int index = database.objects.FindIndex(data => data.ID == ID);
        GameObject obj = Objectcontents[index].gameObject;
        int currentCount = placedGameObjects.Where(data => data.PlacementData.ID == ID).Count();
        int maxCount = database.objects[index].MaxBuildCount;
        if (currentCount >= maxCount)
        {
            obj.GetComponent<Button>().interactable = false;
        }
        else
        {
            obj.GetComponent<Button>().interactable = true;
        }
        Objectcontents[index].leftCount = maxCount - currentCount;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"x{maxCount - currentCount}";

        return maxCount - currentCount;
    }

    public void ShowObjectList()
    {
        SetObjectList();
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

    public void SetObjectList()
    {
        if (Objectcontents.Count <= 0)
        {
            //if(dataBase == null)
            //{
            //    dataBase = placementSystem.Database;
            //}

            foreach (var data in dataBase.objects)
            {
                PlacementUIObjectInfo uiObjInfo = Instantiate(ObjectContentPrefeb, Objectcontent.transform);
                uiObjInfo.SetUIObjectInfo(data, placementSystem);
                Objectcontents.Add(uiObjInfo);
            }
        }
        
    }

    public void OnChangeObjectList()
    {
        Toggle toggle = toggles.ActiveToggles().FirstOrDefault();

        
        if (toggle.name.Equals("All"))
        {
            foreach (var item in Objectcontents)
            {
                item.gameObject.SetActive(true);
                if (item.leftCount <= 0)
                {
                    item.GetComponent<Button>().interactable = false;
                    continue;
                }
            }
            return;
        }

        foreach (var item in Objectcontents)
        {
            if(item.placementInfo.Kind.ToString().Equals(toggle.name))
            {
                item.gameObject.SetActive(true);
                if (item.leftCount <= 0)
                {
                    item.GetComponent<Button>().interactable = false;
                    continue;
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        } 

    }
    public void OnOpenBuildInfo(PlacementObjectInfo placementInfo)
    {
        buildInfo.gameObject.SetActive(true);
        buildInfo.SetUIInfo(placementInfo, placementSystem.SelectedObject);
    }

    public void OnOpenObjectInfo(PlacementObjectInfo placementInfo)
    {
        objectInfo.gameObject.SetActive(true);
        objectInfo.SetUIInfo(placementInfo, placementSystem.SelectedObject);
    }

    public void OnShowModeSelectButton(bool show)
    {
        modeSelectButtons.SetActive(show);
    }

    public void OnOpenUpgradeInfo(PlacementObjectInfo placementInfo)
    {
        upgradeUI.gameObject.SetActive(true);
        upgradeUI.SetUIInfo(placementInfo, placementSystem.SelectedObject);
    }
}
