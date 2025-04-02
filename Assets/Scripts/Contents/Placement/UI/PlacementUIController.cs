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

    [SerializeField] // 배치 / 취소 버튼
    private GameObject placementUI;
    [SerializeField] // 삭제버튼
    private GameObject distroyButton;
    [SerializeField] // 건설물 목록 UI (항목별) 
    private List<PlacementUIObjectInfo> Objectcontents = new();
    [SerializeField] // 건설물 목록 UI (항목별) 
    private GameObject Objectcontent;
    [SerializeField] // 건설물 목록 프리팹
    private PlacementUIObjectInfo ObjectContentPrefeb;
    [SerializeField] // 건설물 목록 UI
    private GameObject ObjectListUi;
    [SerializeField]
    private ToggleGroup toggles;
    [SerializeField]
    private BuildInfoUI infoUI; // 아이템 정보 UI


    private void Awake()
    {
        placementSystem = GetComponent<PlacementSystem>();
        SetObjectList();
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
        GameObject obj = Objectcontents[ID].gameObject;
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
        Objectcontents[ID].leftCount = maxCount - currentCount;
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

    public void SetObjectList()
    {
        foreach(var data in dataBase.objects)
        {
            PlacementUIObjectInfo uiObjInfo = Instantiate(ObjectContentPrefeb, Objectcontent.transform);
            uiObjInfo.SetUIObjectInfo(data, placementSystem);
            Objectcontents.Add(uiObjInfo);
        }
        
    }

    public void OnChangeObjectList()
    {
        Toggle toggle = toggles.ActiveToggles().FirstOrDefault();

        
        if (toggle.name.Equals("All"))
        {
            foreach (var item in Objectcontents)
            {
                if (item.leftCount <= 0)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }
                item.gameObject.SetActive(true);
            }
            return;
        }

        foreach (var item in Objectcontents)
        {
            if(item.placementInfo.Kind.ToString().Equals(toggle.name))
            {
                if(item.leftCount <= 0)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        } 

    }
    public void OnOpenObjectInfo(PlacementObjectInfo placementInfo)
    {
        infoUI.gameObject.SetActive(true);
        infoUI.SetUIInfo(placementInfo);
    }
}
