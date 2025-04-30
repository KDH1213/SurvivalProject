using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIItemDropInfo : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private TextMeshProUGUI amountText;

    public UnityAction diableAction;

    public int ItemID {  get; private set; }
    private int amount;
    private float disableTime = 0f;
    private static readonly string amountFormat = " X {0}";

    private void OnDisable()
    {
        diableAction?.Invoke();
        amount = 0;
        ItemID = 0;
    }

    public void SetDropItemInfo(Sprite icon, int id , int amount, float disableTime)
    {
        itemIcon.sprite = icon;
        this.amount = amount;
        ItemID = id;
        amountText.text = string.Format(amountFormat, amount.ToString());

        this.disableTime = disableTime;

        gameObject.SetActive(true);
    }

    public void AddDropItemCount(int amount)
    {
        this.amount += amount;
        amountText.text = string.Format(amountFormat, this.amount.ToString());
    }

    private void Update()
    {
        if(disableTime < Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
