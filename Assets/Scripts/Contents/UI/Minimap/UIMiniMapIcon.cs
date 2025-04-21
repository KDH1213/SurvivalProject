using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiniMapIcon : MonoBehaviour
{
    private Transform ownerTransform;
    private RectTransform rectTransform;

    private Vector2 mapRatio;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.anchoredPosition = ownerTransform.position.ConvertVector2() * mapRatio;
    }

    public void SetOnwer(Transform owner)
    {
        ownerTransform = owner;
    }

    public void ConvertPosition(Vector2 ratio)
    {
        rectTransform.anchoredPosition = ownerTransform.position.ConvertVector2() * ratio;
        mapRatio = ratio;
    }
}
