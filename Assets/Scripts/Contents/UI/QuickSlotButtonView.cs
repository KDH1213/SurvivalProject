using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuickSlotButtonView : MonoBehaviour
{
    [SerializeField]
    private Button slotButton;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private QuickSlotSocket equipmentSocket;

    [SerializeField]
    private GameObject amountView;

    public UnityEvent onUseItemEvnet;

    public void Awake()
    {
        onUseItemEvnet.AddListener(equipmentSocket.OnUseItem);
    }

    private void Empty()
    {
        itemImage.sprite = null;
        itemImage.color = new Color(1, 1, 1, 0);
        amountView.gameObject.SetActive(false);
    }

    public void OnSetItemInfo()
    {
        if(equipmentSocket.ItemData == null)
        {
            Empty();
        }
        else
        {
            itemImage.sprite = equipmentSocket.ItemData.ItemImage;
            itemImage.color = Color.white;
            amountText.text = equipmentSocket.Amount.ToString();
            amountView.gameObject.SetActive(true);
        }
    }

    public void OnSetAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void OnUseItem()
    {
        onUseItemEvnet?.Invoke();
    }

}
