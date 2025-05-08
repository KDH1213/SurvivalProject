using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoUI : MonoBehaviour
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
    private TextMeshProUGUI objectInfo1;
    [SerializeField]
    private TextMeshProUGUI objectInfo2;
    [SerializeField]
    private Button upgrade;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        upgrade.onClick.RemoveAllListeners();

        var data = DataTableManager.StructureTable.Get(selectedObject.ID);
        var stringTable = DataTableManager.StringTable;

        objectName.text = objInfo.Name;
        objectImage.sprite = objInfo.Icon;
        objectSize.text = $"{objInfo.Size.x} X {objInfo.Size.y}";
        objectHp.text = $"HP : {selectedObject.Hp}";

        objectInfo1.text = stringTable.Get(data.DescriptID);
        SetStructureDescript(objInfo);

        if (objInfo.NextStructureID == 0)
        {
            upgrade.interactable = false;
        }

        upgrade.onClick.AddListener(() => gameObject.SetActive(false));
        upgrade.onClick.AddListener(() => selectedObject.uiController.OnOpenUpgradeInfo(objInfo));
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }

    private void SetStructureDescript(PlacementObjectInfo info)
    {
        var kind = info.Kind;
        var data = DataTableManager.StructureTable.Get(info.ID);
        var itemData = DataTableManager.ItemTable.Get(data.ItemToProduce);
        switch (kind)
        {
            case StructureKind.Other:
                objectInfo2.text = "기타 건물";
                break;
            case StructureKind.Farm:
                objectInfo2.text = $"{data.ProductionCycle}초마다 {itemData.ItemName} {data.AmountPerProduction}개 생성\r\n";
                break;
            case StructureKind.Turret:
                var turret = info.Prefeb.transform.GetChild(0).GetComponent<TurretStructure>();
                objectInfo2.text = $"공격력 : {turret.damage}\t 공격속도 : {turret.attackTerm}";
                break;
            case StructureKind.Create:
                objectInfo2.text = $"제작 건물";
                break;
            case StructureKind.Storage:
                objectInfo2.text = $"아이템 보관";
                break;
            case StructureKind.Rest:
                objectInfo2.text = $"피로도 감소";
                break;
        }
    }
}
