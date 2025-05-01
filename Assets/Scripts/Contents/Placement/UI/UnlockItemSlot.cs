using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    public void SetItem(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
}
