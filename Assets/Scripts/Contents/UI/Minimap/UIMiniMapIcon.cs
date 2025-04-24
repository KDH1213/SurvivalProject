using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiniMapIcon : MonoBehaviour
{
    private Transform ownerTransform;
    private RectTransform rectTransform;
    private RectTransform minimapRectTransform;

    private Vector2 mapRatio;

    private bool isMarker = false;
    private float maskRadius;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(!isMarker)
        {
            rectTransform.anchoredPosition = ownerTransform.position.ConvertVector2() * mapRatio;
        }
        else
        {
            rectTransform.anchoredPosition = ownerTransform.position.ConvertVector2() * mapRatio;

            var minimapPosition = minimapRectTransform.anchoredPosition;
            var direction = minimapPosition - rectTransform.anchoredPosition;
            if(direction.magnitude > maskRadius)
            {
                direction.Normalize();
                rectTransform.anchoredPosition = minimapPosition + (direction * maskRadius) - rectTransform.rect.center;
            }
        }
    }

    public void SetOwner(Transform owner)
    {
        ownerTransform = owner;
    }

    public void ConvertPosition(Vector2 ratio)
    {
        rectTransform.anchoredPosition = ownerTransform.position.ConvertVector2() * ratio;
        mapRatio = ratio;
    }

    public void SetMinimapInfo(RectTransform minimapRectTransform, Rect maskRect)
    {
        this.minimapRectTransform = minimapRectTransform;
        maskRadius = maskRect.width;
    }

    public void SetActiveMarker(bool isMarker)
    {
        this.isMarker = isMarker;

        if(isMarker)
        {
            enabled = true;
        }
    }
}
