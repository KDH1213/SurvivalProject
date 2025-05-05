using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryDivisionView : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TextMeshProUGUI totalText;

    [SerializeField]
    private Slider divisionSlide;

    [SerializeField]
    private TextMeshProUGUI divisionCountText;

    [SerializeField]
    private Button plusButton;

    [SerializeField]
    private Button minusButton;

    [SerializeField]
    private Button divisionButton;

    public UnityEvent<int> onDivisionEvent;

    private ItemSlotInfo seleteItemInfo;

    private int currentDivisionCount = 0;
    private int maxDivisionCount = 0;

    private void Awake()
    {
        divisionSlide.onValueChanged.AddListener(OnValueChange);
        plusButton.onClick.AddListener(OnClickPlusButton);
        minusButton.onClick.AddListener(OnClickMinuseButton);
        divisionButton.onClick.AddListener(OnDivision);
    }

    private void OnEnable()
    {
        if(seleteItemInfo == null)
        {
            return;
        }

        totalText.text = seleteItemInfo.Amount.ToString();
        divisionCountText.text = currentDivisionCount.ToString();

        iconImage.sprite = seleteItemInfo.itemData.ItemImage;

        divisionSlide.minValue = 0;
        divisionSlide.maxValue = seleteItemInfo.Amount;
        maxDivisionCount = seleteItemInfo.Amount;

        currentDivisionCount = 0;
        divisionSlide.value = 0;
        OnCheckButtonInteractable(currentDivisionCount);
    }

    public void OnSetItemInfoEvnet(ItemSlotInfo itemInfo)
    {
        seleteItemInfo = itemInfo;
    }

    public void OnValueChange(float count)
    {
        currentDivisionCount = (int)count;
        divisionCountText.text = currentDivisionCount.ToString();

        OnCheckButtonInteractable(currentDivisionCount);
    }

    private void OnCheckButtonInteractable(int count)
    {
        if (count == 0)
        {
            minusButton.interactable = false;
        }
        else if (count == maxDivisionCount)
        {
            plusButton.interactable = false;
        }
        else
        {
            minusButton.interactable = true;
            plusButton.interactable = true;
        }
    }

    private void OnClickPlusButton()
    {
        ++currentDivisionCount;
        divisionSlide.value = currentDivisionCount;
        divisionCountText.text = currentDivisionCount.ToString();

        OnCheckButtonInteractable(currentDivisionCount);
    }
    private void OnClickMinuseButton()
    {
        --currentDivisionCount;
        divisionSlide.value = currentDivisionCount;
        divisionCountText.text = currentDivisionCount.ToString();

        OnCheckButtonInteractable(currentDivisionCount);
    }

    public void OnDivision()
    {
        onDivisionEvent?.Invoke(currentDivisionCount);
    }
    //[SerializeField]
    //private Button 

}
