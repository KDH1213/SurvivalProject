//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.InputSystem.UI;
//using UnityEngine.UI;

//public class PlayerInventory : MonoBehaviour, IDragHandler
//{
//    public static int maxSlot = 20;

//    [SerializeField]
//    private TextMeshProUGUI infomationText;
//    public List<ItemData> itemList = new List<ItemData>(maxSlot);

//    public Slot prefabSlot;
//    public ScrollRect scrollRect;

//    [SerializeField]
//    private Transform slotParent;
//    [SerializeField]
//    private Slot[] slots;

//    public int SelectedSlotIndex { get; private set; } = -1;

//    public UnityEvent onClickAddButton;
//    public UnityEvent onClickRemoveButton;
//    public UnityEvent onUpdateSlots;

//    private int dragSeletedSlotIndex;
//    private bool isOnDrag = false;

//    private List<Item> tempItemList;

//    public void AddListeners(UnityAction action)
//    {
//        foreach (var slot in slots)
//        {
//            slot.button.onClick.AddListener(action);
//        }
//    }

//    void Awake()
//    {
//        foreach (var item in itemList)
//        {
//            Debug.Log(item);
//        }

//        slots = new Slot[maxSlot];

//        for (int i = 0; i < maxSlot; i++)
//        {
//            var slot = Instantiate(prefabSlot, scrollRect.content);
//            slot.SlotIndex = i;
//            slot.button.onClick.AddListener(() => { SelectedSlotIndex = slot.SlotIndex; });
//            slot.button.onClick.AddListener(ShowInfomation);

//            slot.onDragEnter.AddListener(() =>
//            {
//                isOnDrag = true;
//                dragSeletedSlotIndex = slot.SlotIndex;
//            });

//            slot.onDragExit.AddListener((PointerEventData eventData) =>
//            {
//                var results = new List<RaycastResult>();
//                EventSystem.current.RaycastAll(eventData, results);

//                foreach (var result in results)
//                {
//                    var uiItemSlot = result.gameObject.GetComponent<Slot>();
//                    if (uiItemSlot != null)
//                    {
//                        if (itemList[dragSeletedSlotIndex].ItemName == itemList[uiItemSlot.SlotIndex].ItemName)
//                        {
//                            //slots[uiItemSlot.SlotIndex].Amount += slots[dragSeletedSlotIndex].Amount;
//                            //slots[uiItemSlot.SlotIndex].Item.mount += slots[dragSeletedSlotIndex].Item.amount;

//                            itemList[dragSeletedSlotIndex] = null;

//                            itemList = itemList.Where(item => item != null).ToList();
//                        }
//                        else
//                        {
//                            (itemList[dragSeletedSlotIndex], itemList[uiItemSlot.SlotIndex]) = (itemList[uiItemSlot.SlotIndex], itemList[dragSeletedSlotIndex]);
//                        }

//                        UpdateSlots(itemList);
//                        break;
//                    }

//                    if (results.Count == 0)
//                    {
//                        itemList.Add(itemList[dragSeletedSlotIndex]);
//                        itemList[dragSeletedSlotIndex] = null;
//                        itemList = itemList.Where(item => item != null).ToList();
//                        UpdateSlots(itemList);
//                    }
//                }

//                isOnDrag = false;
//                dragSeletedSlotIndex = -1;
//            });
//            slots[i] = slot;
//        }

//        UpdateSlots(itemList);
//    }

//    //private void OnEnable()
//    //{
//    //    tempItemList = SaveLoadManager.Data.itemList.ToList();
//    //    UpdateSlots(tempItemList);
//    //}

//    //private void OnDisable()
//    //{
//    //    SaveLoadManager.Data.itemList = tempItemList.ToList();
//    //    SaveLoadManager.Save();
//    //}

//    private void UpdateSlots(List<ItemData> items)
//    {
//        for (int i = 0; i < maxSlot; i++)
//        {
//            if (i < items.Count && items[i] != null)
//            {
//                slots[i].Item = items[i];
//                if (!slots[i].useSetAmout)
//                {
//                    //slots[i].Item.amount = items[i].amount;
//                    slots[i].SetAmount();
//                }
//                //slots[i].amountText.text = $"{slots[i].Item.amount}";
//            }
//            else
//            {
//                slots[i].Item = null;
//                slots[i].Amount = 0;
//                slots[i].amountText.text = "0";
//            }
//        }
//    }


//    //public void OnClickRemove()
//    //{
//    //    if (SelectedSlotIndex == -1)
//    //        return;

//    //    // Data에 지워주는 메소드를 제작해야 함.
//    //    // 해당 메소드에서 유효성 검사를 진행
//    //    tempItemList.Remove(slots[SelectedSlotIndex].Item);

//    //    UpdateSlots(tempItemList);

//    //    // UpdateSlots(tempItemList);
//    //    onClickRemoveButton?.Invoke();
//    //}

//    public void OnClickAdd(ItemData itemData, int amount = 1)
//    {
//        int index;

        
//    }

//    public void ShowInfomation()
//    {
//        ItemData item = slots[SelectedSlotIndex].Item;

//        if (item != null)
//        {
//            //infomationText.text = $"Name: {item.itemName}\nInfo: {item.itemInfomation}\nType: {item.type}";
//        }
//        else
//        {
//            infomationText.text = $"Name:\nInfo:\nType:";
//        }
//    }

//    public void RemoveItem()
//    {
//        ItemData item = slots[SelectedSlotIndex].Item;

//        itemList.Remove(item);
//        UpdateSlots(itemList);

//        onClickRemoveButton?.Invoke();
//    }

//    public void ResetInfomation()
//    {
//        infomationText.text = $"Name:\nInfo:\nType:";
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        //var results = new List<RaycastResult>();
//        //EventSystem.current.RaycastAll(eventData, results);

//        //foreach(var item in results)
//        //{
//        //    Debug.Log(item.gameObject.name);
//        //}
//    }


//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        var selectedObject = EventSystem.current.currentSelectedGameObject;
//        var uiItemSlot = selectedObject?.GetComponent<Slot>();

//        if (uiItemSlot != null)
//        {
//            dragSeletedSlotIndex = uiItemSlot.SlotIndex;
//            isOnDrag = true;
//        }
//        else
//        {
//            isOnDrag = false;
//            dragSeletedSlotIndex = -1;
//        }
//    }


//    public void OnDragEnterItemSlot()
//    {
//        //dragSeletedSlotIndex = uiItemSlot.SlotIndex;
//        //isOnDrag = true;
//    }

//    public void OnDragExitItemSlot()
//    {

//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        if (!isOnDrag)
//        {
//            isOnDrag = false;
//            dragSeletedSlotIndex = -1;
//        }


//        var selectedObject = EventSystem.current.currentSelectedGameObject;
//        var uiItemSlot = selectedObject?.GetComponent<Slot>();

//        if (uiItemSlot == null)
//            return;

//        (itemList[dragSeletedSlotIndex], itemList[uiItemSlot.SlotIndex]) = (itemList[uiItemSlot.SlotIndex], itemList[dragSeletedSlotIndex]);
//        UpdateSlots(itemList);
//        isOnDrag = false;
//        dragSeletedSlotIndex = -1;
//    }
//}
