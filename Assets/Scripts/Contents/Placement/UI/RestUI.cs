using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private TextMeshProUGUI objectName;
    [SerializeField]
    private TextMeshProUGUI objectSize;
    [SerializeField]
    private TextMeshProUGUI objectHp;

    [SerializeField]
    private TextMeshProUGUI objectDescription;
    [SerializeField]
    private TextMeshProUGUI reduceFatigue; 
    [SerializeField]
    private TextMeshProUGUI restTime;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Button restEndButton;
    [SerializeField]
    private List<GameObject> disableUI;

    private RestStructure currentStructure;
    private float maxFatigue;
    private float perMinusFatigue;
    private float leftTime;

    private readonly string reduceFormat = "피로도 {0}% 감소";
    private readonly string timeFormat = "{0}분 휴식";

    private void Update()
    {
        if(currentStructure != null)
        {

            reduceFatigue.text = string.Format(reduceFormat, slider.value);
            leftTime = Mathf.FloorToInt(maxFatigue * slider.value * 0.01f / perMinusFatigue);
            restTime.text = string.Format(timeFormat, leftTime);
        }
    }

    public void SetUI(PlacementObjectInfo objInfo, GameObject target, RestStructure selectedObject)
    {
        maxFatigue = FatigueStat.MaxValue - GameObject.FindWithTag(Tags.Player).GetComponent<FatigueStat>().Value;
        slider.maxValue = 100;
        slider.minValue = 10;

        var data = DataTableManager.StructureTable.Get(selectedObject.ID);
        var stringTable = DataTableManager.StringTable;

        perMinusFatigue = data.FatigueReductionPerMinute;

        objectName.text = objInfo.Name;
        objectImage.sprite = objInfo.Icon;
        objectSize.text = $"{objInfo.Size.x} X {objInfo.Size.y}";
        objectHp.text = $"HP : {selectedObject.Hp}";
        objectDescription.text = stringTable.Get(data.DescriptID);

        currentStructure = selectedObject;
        restEndButton.onClick.AddListener(() => OnEndRest());

    }

    public void Rest()
    {
        foreach(var ui in disableUI)
        {
            ui.SetActive(false);
        }
        restEndButton.gameObject.SetActive(true);
        currentStructure.SetRest(leftTime);
        currentStructure.endRest.AddListener(() => OnEndRest());
    }

    public void OnEndRest()
    {
        foreach (var ui in disableUI)
        {
            ui.SetActive(true);
        }
        restEndButton.gameObject.SetActive(false);
        currentStructure.OnEndRest();
    }
}
