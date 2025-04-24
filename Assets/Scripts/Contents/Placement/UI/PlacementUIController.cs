using DG.Tweening;
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
    //private PlacementObjectList dataBase;

    [SerializeField] // ��ġ / ��� ��ư
    private GameObject placementUI;
    [SerializeField] // ������ư
    private GameObject distroyButton;
    [SerializeField] // �Ǽ��� ��� UI (�׸�) 
    private List<PlacementUIObjectInfo> objectcontents = new();
    [SerializeField] // �Ǽ��� ��� UI (�׸�) 
    private GameObject objectcontent;
    [SerializeField] // �Ǽ��� ��� ������
    private PlacementUIObjectInfo objectContentPrefeb;
    [SerializeField] // �Ǽ��� ��� UI
    private GameObject objectListUi;
    [SerializeField] // ������ ����Ʈ ��
    private ToggleGroup toggles;
    [SerializeField] 
    private GameObject modeSelectButtons;
    [SerializeField]
    private BuildInfoUI buildInfo; // �Ǽ��� �Ǽ� ���� UI
    [SerializeField]
    private ObjectInfoUI objectInfo; // �Ǽ��� ���� UI
    [SerializeField]
    private UpgradeUI upgradeUI; // ���׷��̵� UI
    [SerializeField]
    private FarmInfoUi farmUI; // ���� UI
    [SerializeField]
    private UpgradeFarmUI upgradeFarmUI; // ���� ���׷��̵� UI
    [SerializeField]
    private CreateItemUI createItemUI; // ������ ���� UI
    [SerializeField]
    private StorageUI storageUI; // â�� UI

    private void Awake()
    {
        placementSystem = GetComponent<PlacementSystem>();
    }

    private void Start()
    {
        //dataBase = placementSystem.Database;
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
        int listIndex = objectcontents.FindIndex(data => data.placementInfo.SubType == database.objects[index].SubType);
        GameObject obj = objectcontents[listIndex].gameObject;
        int currentCount = placedGameObjects.Where(data => data.PlacementData.ID == ID).Count();
        int maxCount = placementSystem.buildCount.buildCounts[database.objects[index].SubType];
        if (currentCount >= maxCount)
        {
            obj.GetComponent<Button>().interactable = false;
        }
        else
        {
            obj.GetComponent<Button>().interactable = true;
        }
        objectcontents[listIndex].leftCount = maxCount - currentCount;
        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"x{maxCount - currentCount}";

        return maxCount - currentCount;
    }

    public void ShowObjectList()
    {
        
        RectTransform rectTran = objectListUi.GetComponent<RectTransform>();
        Vector3 uiPos = Camera.main.WorldToScreenPoint(objectListUi.transform.position);
        objectListUi.transform.DOMoveY(0, 0.3f);
    }
    public void StopShowObjectList()
    {
        RectTransform rectTran = objectListUi.GetComponent<RectTransform>();
        Vector3 uiPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -rectTran.offsetMax.y));

        objectListUi.transform.DOLocalMoveY(objectListUi.transform.localPosition.y - rectTran.offsetMax.y, 0.3f);

    }

    public void SetObjectList(PlacementObjectList database)
    {
        if (objectcontents.Count <= 0)
        {
            foreach (var data in database.objects)
            {
                if(data.Rank != 1)
                {
                    continue;
                }
                PlacementUIObjectInfo uiObjInfo = Instantiate(objectContentPrefeb, objectcontent.transform);
                uiObjInfo.SetUIObjectInfo(data, placementSystem);
                objectcontents.Add(uiObjInfo);
                OnSetObjectListUi(database, data.ID, placementSystem.PlacedGameObjects);
            }
        }
        
    }

    public void OnChangeObjectList()
    {
        Toggle toggle = toggles.ActiveToggles().FirstOrDefault();

        
        if (toggle.name.Equals("All"))
        {
            foreach (var item in objectcontents)
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

        foreach (var item in objectcontents)
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
        SetObjectList(placementSystem.Database);
        modeSelectButtons.SetActive(show);
    }

    public void OnOpenUpgradeInfo(PlacementObjectInfo placementInfo)
    {
        if(placementInfo.Kind == StructureKind.Farm)
        {
            upgradeFarmUI.gameObject.SetActive(true);
            upgradeFarmUI.SetUIInfo(placementInfo, placementSystem.SelectedObject);
        }
        else
        {
            upgradeUI.gameObject.SetActive(true);
            upgradeUI.SetUIInfo(placementInfo, placementSystem.SelectedObject);
        }
        
    }
    public void OnOpenFarmInfo(GameObject target, int id, ProduceStructure produceInfo)
    {
        if(placementSystem == null)
        {
            placementSystem = GetComponent<PlacementSystem>();
        }
        farmUI.gameObject.SetActive(true);
        int index = placementSystem.Database.objects.FindIndex(x => x.ID == id);
        PlacementObjectInfo data = placementSystem.Database.objects[index];
        farmUI.SetUIInfo(data, target, produceInfo);
    }

    public void OnOpenCreateItemUI(GameObject target, CreateStructure structure)
    {
        createItemUI.gameObject.SetActive(true);
        createItemUI.SetUI(target, structure);
    }

    public void OnOpenStorageUI(GameObject target, StorageStructure structure)
    {
        storageUI.gameObject.SetActive(true);
        storageUI.SetUI(target, structure);
    }
}
