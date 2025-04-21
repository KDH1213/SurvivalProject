using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiniMapIcon : MonoBehaviour
{
    private Transform ownerTransform;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetOnwer(Transform owner)
    {
        ownerTransform = owner;
    }

    public void ConvertPosition(Vector2 ratio)
    {
        rectTransform.anchoredPosition = ownerTransform.position.ConvertVector2() * ratio;
    }
}
