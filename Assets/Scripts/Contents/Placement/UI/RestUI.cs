using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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

    private RestStructure currentStructure;

    private readonly string reduceFormat = "피로도 {0}% 감소";
    private readonly string timeFormat = "{0}분 휴식";

    private void Update()
    {
        if(currentStructure != null)
        {
            reduceFatigue.text = string.Format(reduceFormat, slider.value);
            restTime.text = string.Format(timeFormat, slider.value * 5);
        }
    }

    public void SetUI(PlacementObjectInfo objInfo, GameObject target, RestStructure selectedObject)
    {
        //GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>().CurrentStatTable[StatType.fati];
        slider.maxValue = 100;
        slider.minValue = 10;

        var data = DataTableManager.StructureTable.Get(selectedObject.ID);
        var stringTable = DataTableManager.StringTable;

        objectName.text = objInfo.Name;
        objectImage.sprite = objInfo.Icon;
        objectSize.text = $"{objInfo.Size.x} X {objInfo.Size.y}";
        objectHp.text = $"HP : {selectedObject.Hp}";
        objectDescription.text = stringTable.Get(data.DescriptID);

        currentStructure = selectedObject;
    }

    public void Rest()
    {
        currentStructure.SetRest(slider.value * 5);
    }
}
