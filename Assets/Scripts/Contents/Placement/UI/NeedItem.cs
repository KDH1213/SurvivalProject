using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedItem : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI needCountTxt;

    public void SetNeedItem(Sprite image, int needCount, int hasCount)
    {
        itemImage.sprite = image;
        needCountTxt.text = $"{hasCount} / {needCount}";
        if(hasCount < 0)
        {
            needCountTxt.text = $"{needCount}";
        }
    }
}
