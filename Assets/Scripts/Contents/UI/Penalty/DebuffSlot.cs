using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebuffSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Image icon;

    public string DebuffName { get; private set; }
    public string Descript { get; private set; }

    public UnityEvent<DebuffSlot> onPointerEnterEvent;
    public UnityEvent<DebuffSlot> onPointerExitEvent;

    public void SetDebuffInfo(Sprite sprite, string name, string descript)
    {
        icon.sprite = sprite;
        DebuffName = name;
        Descript = descript;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnterEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExitEvent?.Invoke(this);
    }

}
