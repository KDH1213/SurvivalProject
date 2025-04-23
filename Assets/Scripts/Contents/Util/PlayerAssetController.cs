using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetController : MonoBehaviour
{
    [SerializeField]
    private ArmorMeshData armorMeshData;

    [SerializeField]
    private List<SkinnedMeshRenderer> helmetMeshRendererList = new List<SkinnedMeshRenderer>();
    [SerializeField]
    private List<SkinnedMeshRenderer> armorMeshRendererList = new List<SkinnedMeshRenderer>();

    [SerializeField]
    private List<SkinnedMeshRenderer> pantsMeshRendererList = new List<SkinnedMeshRenderer>();

    [SerializeField]
    private List<SkinnedMeshRenderer> shoesMeshRendererList = new List<SkinnedMeshRenderer>();

    private List<List<SkinnedMeshRenderer>> skinnedMeshRendererLists = new List<List<SkinnedMeshRenderer>>();

    private void Awake()
    {
        skinnedMeshRendererLists.Add(helmetMeshRendererList);
        skinnedMeshRendererLists.Add(armorMeshRendererList);
        skinnedMeshRendererLists.Add(pantsMeshRendererList);
        skinnedMeshRendererLists.Add(shoesMeshRendererList);
    }

    public void OnEquipmentItem(ItemData itemData)
    {
        if(itemData.ItemType == ItemType.Weapon)
        {
            OnEquipmentWeapon();
        }
        else if(itemData.ItemType == ItemType.Armor)
        {
           var armorData = DataTableManager.ArmorTable.Get(itemData.ID);
            OnEquipmentArmor(armorData);
        }
    }

    public void OnUnEquipmentItem(ItemData itemData)
    {
        if (itemData.ItemType == ItemType.Weapon)
        {
        }
        else if (itemData.ItemType == ItemType.Armor)
        {
            var armorData = DataTableManager.ArmorTable.Get(itemData.ID);
            OnUnEquipmentArmor(armorData);
        }
    }

    private void OnEquipmentArmor(ArmorData armorData)
    {
        var meshList = armorMeshData.GetMeshList(armorData.ArmorType, armorData.ID);
        int index = (int)armorData.ArmorType - 1;

        for (int i = 0; i < helmetMeshRendererList.Count; ++i)
        {
            skinnedMeshRendererLists[index][i].sharedMesh = meshList[i];
        }
    }

    private void OnEquipmentWeapon()
    {

    }

    private void OnUnEquipmentArmor(ArmorData armorData)
    {
        var meshList = armorMeshData.GetMeshList(armorData.ArmorType, armorData.ID);
        int index = (int)armorData.ArmorType - 1;

        for (int i = 0; i < helmetMeshRendererList.Count; ++i)
        {
            skinnedMeshRendererLists[index][i].sharedMesh = meshList[i];
        }
    }

    private void OnUnEquipmentWeapon()
    {

    }
}
