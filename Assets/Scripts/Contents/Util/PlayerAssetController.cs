using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetController : MonoBehaviour
{
    [SerializeField]
    private Transform rightHandTransform;

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

    private Dictionary<int, GameObject> weaponTable = new Dictionary<int, GameObject>();

    private void Awake()
    {
        Initialized();
    }

    private void Initialized()
    {
        skinnedMeshRendererLists.Clear();
        skinnedMeshRendererLists.Add(helmetMeshRendererList);
        skinnedMeshRendererLists.Add(armorMeshRendererList);
        skinnedMeshRendererLists.Add(pantsMeshRendererList);
        skinnedMeshRendererLists.Add(shoesMeshRendererList);
    }

    public void OnEquipmentItem(ItemData itemData)
    {
        if (itemData.ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(itemData.ID);
            OnEquipmentWeapon(weaponData);
        }
        else if (itemData.ItemType == ItemType.Armor)
        {
            var armorData = DataTableManager.ArmorTable.Get(itemData.ID);
            OnEquipmentArmor(armorData);
        }
    }

    public void OnUnEquipmentItem(ItemData itemData)
    {
        if (itemData.ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(itemData.ID);
            OnUnEquipmentWeapon(weaponData);
        }
        else if (itemData.ItemType == ItemType.Armor)
        {
            var armorData = DataTableManager.ArmorTable.Get(itemData.ID);
            var meshList = armorMeshData.defalutTable[armorData.ArmorType];
            int index = (int)armorData.ArmorType - 1;

            for (int i = 0; i < helmetMeshRendererList.Count; ++i)
            {
                skinnedMeshRendererLists[index][i].sharedMesh = meshList[i];
            }
        }
    }

    private void OnEquipmentArmor(ArmorData armorData)
    {
        var meshList = armorMeshData.GetMeshList(armorData.ArmorType, armorData.ID);

        if (meshList == null)
        {
            return;
        }

        int index = (int)armorData.ArmorType - 1;

        if(skinnedMeshRendererLists.Count == 0)
        {
            Initialized();
        }

        for (int i = 0; i < helmetMeshRendererList.Count; ++i)
        {
            skinnedMeshRendererLists[index][i].sharedMesh = meshList[i];
        }
    }

    private void OnEquipmentWeapon(WeaponData weaponData)
    {
        GameObject weapon = null;
        if(weaponTable.TryGetValue(weaponData.ItemID, out weapon))
        {
            weapon.SetActive(true);
        }
        else
        {
            weapon = Instantiate(weaponData.WeaponPrefab, rightHandTransform);
            weapon.GetComponent<WeaponSocketController>().OnInitialized();
            weaponTable.Add(weaponData.ItemID, weapon);
        }
    }

    private void OnUnEquipmentWeapon(WeaponData weaponData)
    {
        if (weaponTable.TryGetValue(weaponData.ItemID, out var weapon))
        {
            weapon.SetActive(false);
        }
    }

    private void OnUnEquipmentArmor(ArmorData armorData)
    {
        var meshList = armorMeshData.GetMeshList(armorData.ArmorType, armorData.ID);
        int index = (int)armorData.ArmorType - 1;

        if (skinnedMeshRendererLists.Count == 0)
        {
            Initialized();
        }

        for (int i = 0; i < helmetMeshRendererList.Count; ++i)
        {
            skinnedMeshRendererLists[index][i].sharedMesh = meshList[i];
        }
    }
}
