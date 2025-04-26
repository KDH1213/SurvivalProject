using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICheatModeItemData : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button createButton;

    private ItemData itemData;

    public UnityAction<DropItemInfo> onCreateAction;

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        iconImage.sprite = itemData.ItemImage;
        createButton.onClick.AddListener(OnCreateItem);
    }

    public void OnCreateItem()
    {
        if (int.TryParse(inputField.text, out var count))
        {
            if (itemData != null)
            {
                DropItemInfo dropItemInfo = new DropItemInfo();
                dropItemInfo.id = itemData.ID;
                dropItemInfo.itemData = itemData;
                dropItemInfo.amount = count;

                onCreateAction?.Invoke(dropItemInfo);
                // playerInentory.AddItem(dropItemInfo);
            }
        }

        inputField.text = string.Empty;
    }
}
