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

    [SerializeField]
    private float lifeTime = 0.5f;

    private float disableTime = 0f;
    private static readonly string amountFormat = " X {0}";

    private void OnDisable()
    {
        diableAction?.Invoke();
    }

    public void SetDropItemInfo(Sprite icon, int amount)
    {
        itemIcon.sprite = icon;
        amountText.text = string.Format(amountFormat, amount.ToString());

        disableTime = Time.time + lifeTime;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if(disableTime < Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
