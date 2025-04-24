using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    private GameObject iconPrefab;

    [SerializeField]
    private GameObject mapPlane;

    [SerializeField] 
    private RectTransform masktransform;

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
        createIcon.GetComponent<Image>().sprite = createObject.Icon;

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

    public void SetMarker(UIMiniMapIcon uIMiniMapIcon)
    {
        uIMiniMapIcon.SetMinimapInfo(mapRectTransform, masktransform.rect);
        uIMiniMapIcon.SetActiveMarker(true);
    }

    private void Update()
    {
        if(!ReferenceEquals(targetTransform, null))
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
