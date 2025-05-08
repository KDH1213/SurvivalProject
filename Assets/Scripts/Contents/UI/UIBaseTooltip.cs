using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBaseTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject tooltipView;

    [SerializeField]
    private TextMeshProUGUI[] tooltipTexts;

    [SerializeField]
    private int[] tooltipIds;

    private void Awake()
    {
        var count = tooltipTexts.Length;
        for (int i = 0; i < count; i++)
        {
            tooltipTexts[i].text = DataTableManager.StringTable.Get(tooltipIds[i]);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipView.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipView.SetActive(false);
    }
}
