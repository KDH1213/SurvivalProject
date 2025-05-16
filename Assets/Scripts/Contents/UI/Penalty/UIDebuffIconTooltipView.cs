using TMPro;
using UnityEngine;

public class UIDebuffIconTooltipView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private RectTransform rectTransform;

    public void SetDebuffInfo(DebuffSlot targetDebuffSlot)
    {
        var targetRectTransform = targetDebuffSlot.GetComponent<RectTransform>();
        var position = targetRectTransform.anchoredPosition;
        position.x += targetRectTransform.sizeDelta.x;
        position.y += 50f;

        rectTransform.anchoredPosition = position;
        nameText.text = targetDebuffSlot.DebuffName;
        descriptionText.text = targetDebuffSlot.Descript;



    }
}
