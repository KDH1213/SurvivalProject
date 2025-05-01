using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    private GameObject iconPrefab;

    [SerializeField]
    private GameObject questIconPrefab;

    [SerializeField]
    private GameObject mapPlane;

    [SerializeField] 
    private RectTransform masktransform;

    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private MiniMapQuestIconController miniMapQuestIconController;

    [SerializeField]
    private Image mapImage;

    private RectTransform mapRectTransform;

    private Vector2 centerrUV = Vector2.one * 0.5f;

    [SerializeField]
    private Vector2 mapScale;

    [SerializeField]
    private Vector2 mapRatio;

    private List<UIQuestIcon> uIQuestIconList = new List<UIQuestIcon>();
    private List<UIQuestIcon> activeUIQuestIconList = new List<UIQuestIcon>();

    private void Awake()
    {
        Vector2 mapSize = mapPlane.transform.localScale.ConvertVector2() * 10f;

        mapRatio.x = mapImage.rectTransform.rect.width / mapSize.x;
        mapRatio.y = mapImage.rectTransform.rect.height / mapSize.y;

        mapSize.x /= mapImage.rectTransform.rect.width;
        mapSize.y /= mapImage.rectTransform.rect.height;

        mapScale = mapSize;

        mapRectTransform = mapImage.rectTransform;
    }
    public void AddObject(MiniMapObject createObject)
    {
        var createIcon = Instantiate(iconPrefab, createPoint);
        var image = createIcon.GetComponent<Image>();
        image.sprite = createObject.Icon;
        image.color = createObject.IconColor;

        var miniMapIcon = createIcon.GetComponent<UIMiniMapIcon>();
        miniMapIcon.SetOwner(createObject.transform);
        miniMapIcon.ConvertPosition(mapRatio);

        createObject.OnActiveEvent.AddListener((_) => { miniMapIcon.gameObject.SetActive(true); });
        createObject.OnDisabledEvent.AddListener((_) => { if(miniMapIcon != null) miniMapIcon.gameObject.SetActive(false); });

        if (createObject.IsStatic)
        {
            miniMapIcon.enabled = false;
        }
        else
        {
            miniMapIcon.enabled = true;
        }

        if(createObject.IsMarker)
        {
            SetMarker(miniMapIcon);
        }
    }

    public void AddQuestObject(QuestData questData, ref UnityAction onClearAction)
    {
        int count = uIQuestIconList.Count;
        UIQuestIcon uIQuestIcon = null;

        if (count == 0)
        {
            var createIcon = Instantiate(questIconPrefab, createPoint);

            uIQuestIcon = createIcon.GetComponent<UIQuestIcon>();
            onClearAction = uIQuestIcon.OnQuestClear;

            uIQuestIcon.OnDisabledEvent.AddListener((icon) => 
            {
                uIQuestIconList.Add(icon); 
                activeUIQuestIconList.Remove(icon); 
            });
        }
        else
        {
            uIQuestIcon = uIQuestIconList[count - 1];
            uIQuestIconList.Remove(uIQuestIcon);
        }

        activeUIQuestIconList.Add(uIQuestIcon);
        uIQuestIcon.SetQuestInfo(questData.GetTargetPosition().ConvertVector2(), mapRatio, questData.UseQuestMarkRangeInGame == 1, questData.QuestAreaRadius);
        uIQuestIcon.SetMinimapInfo(targetTransform, mapRectTransform, masktransform.rect);
        uIQuestIcon.SetActiveMarker(true);
    }

    public void SetMarker(UIMiniMapIcon uIMiniMapIcon)
    {
        uIMiniMapIcon.SetMinimapInfo(targetTransform, mapRectTransform, masktransform.rect);
        uIMiniMapIcon.SetActiveMarker(true);
    }

    private void LateUpdate()
    {
        if (!ReferenceEquals(targetTransform, null))
        {
            var position = ConvertPosition(targetTransform.position);
            mapRectTransform.anchoredPosition = -position;
        }
    }

    private Vector2 ConvertPosition(Vector3 convertPosition)
    {
        var position = convertPosition.ConvertVector2();
        position *= mapRatio;

        return position;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
}
