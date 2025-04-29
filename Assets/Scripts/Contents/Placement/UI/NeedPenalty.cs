using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedPenalty : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI needCountTxt;

    public void SetNeedPenalty(Sprite image, int needCount)
    {
        itemImage.sprite = image;
        needCountTxt.text = $"{needCount} %";
    }
}
