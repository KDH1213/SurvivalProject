using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        for (int i = 0; i < items.Count && i < slots.Length; i++)
        {
            slots[i].Item = items[i];
        }
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Item = null;
        }
    }

    public void AddItem(Item item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(item);
            FreshSlot();
        }
        else
        {
            print("½½·ÔÀÌ °¡µæ Â÷ ÀÖ½À´Ï´Ù.");
        }
    }
}
