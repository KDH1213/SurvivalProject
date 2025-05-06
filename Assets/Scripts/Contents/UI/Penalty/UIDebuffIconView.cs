using UnityEngine;

public class UIDebuffIconView : MonoBehaviour
{
    [SerializeField]
    private DebuffSlot debuffSlotPrefab;

    [SerializeField]
    private Transform creatPoint;

    [SerializeField]
    private UIDebuffIconTooltipView toolTipView;

    public DebuffSlot CreateDebuffIcon(Sprite sprite, string name, string descript)
    {
        var createSlot = Instantiate(debuffSlotPrefab, creatPoint);
        createSlot.SetDebuffInfo(sprite, name, descript);
        createSlot.onPointerEnterEvent.AddListener(OnEnableTooltipView);
        createSlot.onPointerExitEvent.AddListener(OnDisableTooltipView);

        return createSlot;
    }

    public void OnEnableTooltipView(DebuffSlot targetDebuffSlot)
    {
        toolTipView.SetDebuffInfo(targetDebuffSlot);
        toolTipView.gameObject.SetActive(true);
    }
    public void OnDisableTooltipView(DebuffSlot targetDebuffSlot)
    {
        toolTipView.gameObject.SetActive(false);
    }
}
