using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    private List<UIMiniMapIcon> staticObjectList = new List<UIMiniMapIcon>();
    private List<UIMiniMapIcon> dynamicObjectList = new List<UIMiniMapIcon>();

    [SerializeField]
    private GameObject iconPrefab;

    [SerializeField]
    private GameObject mapPlane;

    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private Image mapImage;

    private RectTransform mapRectTransform;

    private Vector2 centerrUV = Vector2.one * 0.5f;

    [SerializeField]
    private Vector2 mapScale;

    [SerializeField]
    private Vector2 mapRatio;

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
        createIcon.GetComponent<RectTransform>().anchoredPosition = ConvertPosition(createObject.transform.position);
        createIcon.GetComponent<Image>().sprite = createObject.Icon;
        var miniMapDynamicIcon = createIcon.GetComponent<UIMiniMapIcon>();
        miniMapDynamicIcon.SetOnwer(createObject.transform);
        createObject.OnActiveEvent.AddListener((_) => { miniMapDynamicIcon.gameObject.SetActive(true); });
        createObject.OnDisabledEvent.AddListener((_) => { miniMapDynamicIcon.gameObject.SetActive(false); });

        if (createObject.IsStatic)
        {
            staticObjectList.Add(miniMapDynamicIcon);
        }
        else
        {
            dynamicObjectList.Add(miniMapDynamicIcon);
        }
    }

    private void Update()
    {
        if(!ReferenceEquals(targetTransform, null))
        {
            var position = ConvertPosition(targetTransform.position);
            mapRectTransform.anchoredPosition = -position;
        }

        foreach (var dynamicObject in dynamicObjectList)
        {
            dynamicObject.ConvertPosition(mapRatio);
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
