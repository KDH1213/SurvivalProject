using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class Inventory : MonoBehaviour, IDragHandler
{
    private static int maxSlot = 20;

    [SerializeField]
    private TextMeshProUGUI infomationText;
    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private TextMeshProUGUI defenceText;
    [SerializeField]
    private TextMeshProUGUI attackSpeedText;
    [SerializeField]
    private TextMeshProUGUI moveSpeedText;

    private PlayerStats playerStats;

    private ItemData[] items = new ItemData[maxSlot];

    public List<ItemData> itemList;

    [SerializeField]
    private Slot slotPrefab;
    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots = new Slot[maxSlot];
    [SerializeField]
    private Slot[] EquipmentSlots = new Slot[6];

    public int SelectedSlotIndex { get; private set; } = -1;
    public int SelectedEquipmentSlotIndex { get; private set; } = -1;

    public UnityEvent onClickAddButton;
    public UnityEvent onClickRemoveButton;
    public UnityEvent onUpdateSlots;

    private int dragSeletedSlotIndex;
    private bool isOnDrag = false;

    private float totalDefence;
    private float totalAttack;
    private float totalMoveSpeed;
    private float totalAttackSpeed;

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in slots)
        {
            slot.button.onClick.AddListener(action);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            items[i] = itemList[i];
        }

        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            var slot = EquipmentSlots[i];

            EquipmentSlots[i].SlotIndex = i;
            slot.button.onClick.AddListener(() => { SelectedEquipmentSlotIndex = slot.SlotIndex; });
            slot.button.onClick.AddListener(ShowSorketItemInfomation);
        }

        for (int i = 0; i < maxSlot; i++)
        {
            var slot = Instantiate(slotPrefab, scrollRect.content);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() => { SelectedSlotIndex = slot.SlotIndex; });
            slot.button.onClick.AddListener(ShowIventoryItemInfomation);

            slot.onDragEnter.AddListener(() =>
            {
                isOnDrag = true;
                dragSeletedSlotIndex = slot.SlotIndex;
            });

            slot.onDragExit.AddListener((PointerEventData eventData) =>
            {
                if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= items.Length || items[dragSeletedSlotIndex] == null)
                {
                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;
                    return;
                }

                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                foreach (var result in results)
                {
                    var uiItemSlot = result.gameObject.GetComponent<Slot>();
                    if (uiItemSlot == null) continue;

                    int targetSlotIndex = uiItemSlot.SlotIndex;
                    int sourceSlotIndex = dragSeletedSlotIndex;

                    if (targetSlotIndex < 0 || targetSlotIndex >= items.Length) continue;

                    if (targetSlotIndex == sourceSlotIndex)
                    {
                        return;
                    }

                    // 대상 슬롯이 비어 있는 경우
                    if (items[targetSlotIndex] == null)
                    {
                        items[targetSlotIndex] = items[sourceSlotIndex];
                        items[sourceSlotIndex] = null;
                    }
                    else if (items[sourceSlotIndex].ItemName == items[targetSlotIndex].ItemName)
                    {
                        // 같은 아이템일 경우 합치기
                        int totalAmount = items[sourceSlotIndex].Amount + items[targetSlotIndex].Amount;

                        if (totalAmount <= items[targetSlotIndex].MaxAmount)
                        {
                            items[targetSlotIndex].Amount = totalAmount;
                            items[sourceSlotIndex] = null;
                        }
                        else
                        {
                            items[targetSlotIndex].Amount = items[targetSlotIndex].MaxAmount;
                            items[sourceSlotIndex].Amount = totalAmount - items[targetSlotIndex].MaxAmount;
                        }
                    }
                    else
                    {
                        // 서로 다른 아이템 교환
                        var temp = items[targetSlotIndex];
                        items[targetSlotIndex] = items[sourceSlotIndex];
                        items[sourceSlotIndex] = temp;
                    }

                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;

                    UpdateSlots(items);
                    break;
                }

                // 드래그 해제 후에도 아이템이 제대로 이동되지 않았다면 초기화
                isOnDrag = false;
                dragSeletedSlotIndex = -1;
            });
            slots[i] = slot;
        }

        UpdateSlots(items);

        gameObject.SetActive(true);
    }

    private void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        totalDefence = playerStats.Defense;
        totalAttack = playerStats.AttackPower;
        totalMoveSpeed = playerStats.Speed;
        totalAttackSpeed = playerStats.AttackSpeed;

        defenceText.text = $"{totalDefence}";
        attackText.text = $"{totalAttack}";
        moveSpeedText.text = $"{totalMoveSpeed}";
        attackSpeedText.text = $"{totalAttackSpeed}";
    }

    private void UpdateSlots(ItemData[] items)
    {
        for (int i = 0; i < maxSlot; i++)
        {
            if (i < items.Length && items[i] != null)
            {
                if (!slots[i].useSetAmout)
                {
                    slots[i].Item = items[i];
                    SetSlotAmount(i, items[i].Amount, items[i].MaxAmount);
                    slots[i].SetAmount();
                }
                else
                {
                    slots[i].Item = items[i];
                }
                slots[i].amountText.text = $"{slots[i].Item.Amount}";
            }
            else
            {
                slots[i].Item = null;
                slots[i].Amount = 0;
                slots[i].amountText.text = "0";
            }
        }
    }

    private void SetSlotAmount(int index, int amount, int maxAmount)
    {
        slots[index].Amount = amount;
        slots[index].MaxAmount = maxAmount;
    }

    public void ShowIventoryItemInfomation()
    {
        ItemData item = items[SelectedSlotIndex];

        if (item != null)
        {
            infomationText.text = $"Name: {item.ItemName}\n\nInfo: {item.ItemInfomation}\n\nType: {item.type}";
        }
        else
        {
            infomationText.text = "";
        }
    }

    public void ShowSorketItemInfomation()
    {
        ItemData item = EquipmentSlots[SelectedSlotIndex].Item;

        if (item != null)
        {
            infomationText.text = $"Name: {item.ItemName}\n\nInfo: {item.ItemInfomation}\n\nType: {item.type}";
        }
        else
        {
            infomationText.text = "";
        }
    }

    public void ResetInfomation()
    {
        infomationText.text = "";
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < maxSlot;
    }

    //public void RemoveItem()
    //{
    //    ItemData item = slots[SelectedSlotIndex].Item;

    //    itemList.Remove(item);
    //    UpdateSlots(itemList);

    //    onClickRemoveButton?.Invoke();
    //}

    public void EuipItem()
    {
        ItemData item = slots[SelectedSlotIndex].Item;

        if (item.type == itemType.Material)
        {
            return;
        }

        switch (item.type)
        {
            case itemType.Helmet:
                if (EquipmentSlots[0].Item != null)
                {
                    var empty = EquipmentSlots[0].Item;
                    EquipmentSlots[0].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[0].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                UpdatePlayerStat(0);
                break;
            case itemType.Weapon:
                if (EquipmentSlots[1].Item != null)
                {
                    var empty = EquipmentSlots[1].Item;
                    EquipmentSlots[1].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[1].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                UpdatePlayerStat(1);
                break;
            case itemType.Consumable:
                if (EquipmentSlots[2].Item != null)
                {
                    var empty = EquipmentSlots[2].Item;
                    EquipmentSlots[2].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[2].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                UpdatePlayerStat(2);
                break;
            case itemType.Armor:
                if (EquipmentSlots[3].Item != null)
                {
                    var empty = EquipmentSlots[3].Item;
                    EquipmentSlots[3].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[3].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                UpdatePlayerStat(3);
                break;
            case itemType.Pants:
                if (EquipmentSlots[4].Item != null)
                {
                    var empty = EquipmentSlots[4].Item;
                    EquipmentSlots[4].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[4].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                UpdatePlayerStat(4);
                break;
            case itemType.Shoes:
                if (EquipmentSlots[5].Item != null)
                {
                    var empty = EquipmentSlots[5].Item;
                    EquipmentSlots[5].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[5].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                UpdatePlayerStat(5);
                break;
        }
        UpdateSlots(items);
    }

    public void UnEquipItem()
    {
        ItemData item = EquipmentSlots[SelectedEquipmentSlotIndex].Item;

        int index = FindEmptySlotIndex();

        items[index] = item;

        EquipmentSlots[SelectedSlotIndex].Item = null;

        UpdateSlots(items);
    }

    private int FindEmptySlotIndex()
    {
        foreach (var slot in slots)
        {
            if (slot.Item == null)
            {
                return slot.SlotIndex;
            }
        }

        return -1;
    }

    public void UpdatePlayerStat(int index)
    {
        if(EquipmentSlots[index].Item == null)
        {
            return;
        }

        if (EquipmentSlots[index].Item.type != itemType.Weapon && EquipmentSlots[index].Item.type != itemType.Consumable)
        {
            totalDefence += EquipmentSlots[index].Item.Defence;
            playerStats.SetDefenceValue(EquipmentSlots[index].Item.Defence);
        }
        
        if (EquipmentSlots[index].Item.type == itemType.Weapon)
        {
            totalAttack += EquipmentSlots[index].Item.Attack;
            totalAttackSpeed += EquipmentSlots[index].Item.AttackSpeed;

            playerStats.SetAttackPowerValue(EquipmentSlots[index].Item.Attack);
            playerStats.SetAttackSpeedValue(EquipmentSlots[index].Item.AttackSpeed);
        }
        else if(EquipmentSlots[index].Item.type == itemType.Shoes)
        {
            totalMoveSpeed += EquipmentSlots[index].Item.MoveSpeed;
            playerStats.SetMoveSpeedValue(EquipmentSlots[index].Item.MoveSpeed);
        }

        defenceText.text = $"{totalDefence}";
        attackText.text = $"{totalAttack}";
        moveSpeedText.text = $"{totalMoveSpeed}";
        attackSpeedText.text = $"{totalAttackSpeed}";
    }

    public void AddItem(ItemData newItem)
    {
        // 같은 아이템이 있는지 확인
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].ItemName == newItem.ItemName)
            {
                int totalAmount = items[i].Amount + newItem.Amount;

                if (totalAmount <= items[i].MaxAmount)
                {
                    items[i].Amount = totalAmount;
                }
                else
                {
                    items[i].Amount = items[i].MaxAmount;
                    newItem.Amount = totalAmount - items[i].MaxAmount;

                    // 남은 개수로 새 슬롯에 추가
                    int emptyIndex = FindEmptySlotIndex();
                    if (emptyIndex != -1)
                    {
                        items[emptyIndex] = Instantiate(newItem);
                    }
                }

                UpdateSlots(items);
                return;
            }
        }

        // 인벤토리에 같은 아이템이 없다면 빈 슬롯 찾기
        int index = FindEmptySlotIndex();
        if (index != -1)
        {
            items[index] = Instantiate(newItem);
        }
        else
        {
            Debug.Log("인벤토리가 가득 찼습니다!");
        }

        UpdateSlots(items);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //var results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventData, results);

        //foreach(var item in results)
        //{
        //    Debug.Log(item.gameObject.name);
        //}
    }
}
