using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasePointUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hp;
    [SerializeField]
    private Image hpFill;
    [SerializeField]
    private TextMeshProUGUI returnCount;
    [SerializeField]
    private TextMeshProUGUI haveCount;
    [SerializeField]
    private Button returnButton;

    private int totalRelics = 0;
    private GameObject target;
    private Inventory inventory;
    private BaseStructure currentStructure;
    public void SetUI(GameObject target, BaseStructure selectedObject)
    {
        this.target = target;
        inventory = target.GetComponent<PlayerFSM>().PlayerInventory;
        currentStructure = selectedObject;

        SetUIInfo(selectedObject);
    }

    public void OnReturnRelics()
    {
        var relics = inventory.InventroyItemTable.Where(item => item.Value[0].itemData.ItemType == ItemType.Relics);
        foreach (var relic in relics)
        {
            inventory.ConsumeItem(relic.Key, inventory.GetTotalItem(relic.Key));
        }

        currentStructure.returnCount += totalRelics;
        totalRelics = 0;

        SetUIInfo(currentStructure);
    }

    private void SetUIInfo(BaseStructure selectedObject)
    {
        hp.text = selectedObject.Hp.ToString();
        hpFill.fillAmount = selectedObject.Hp / selectedObject.maxHp;
        returnCount.text = $"{selectedObject.returnCount.ToString()} / {selectedObject.maxRelics}";


        var relics = inventory.InventroyItemTable.Where(item => item.Value[0].itemData.ItemType == ItemType.Relics);
        foreach (var relic in relics)
        {
            totalRelics += inventory.GetTotalItem(relic.Key);
        }

        haveCount.text = $"보유 개수 : {totalRelics}";
    }
}
