using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    [field: SerializeField]
    public int MonsterID    { get; set; }
    [field: SerializeField]
    public int Name { get; set; }
    [field: SerializeField]
    public string PrefabName { get; set; }
    [field: SerializeField]
    public int MonsterType { get; set; }

    [field: SerializeField]
    public int AttackType { get; set; }

    [field: SerializeField]
    public float HP { get; set; }
    [field: SerializeField]
    public float AttackPower { get; set; }
    [field: SerializeField]
    public float Defense { get; set; }
    [field: SerializeField]
    public float MovementSpeed { get; set; }
    [field: SerializeField]
    public float AttackSpeed { get; set; }

    [field: SerializeField]
    public float MaxChaseRadius { get; set; }

    [field: SerializeField]
    public float ChaseRadius { get; set; }
    [field: SerializeField]
    public float AttackRadius { get; set; }
    [field: SerializeField]
    public float AttackCooldown { get; set; }

    [field: SerializeField]
    public float ProjectileSpeed { get; set; }
    [field: SerializeField]
    public float LifeTime { get; set; }
    [field: SerializeField]
    public float DropLifeExp { get; set; }
    [field: SerializeField]
    public int AttackEffect { get; set; }
    [field: SerializeField]
    public int ProjectileEffect { get; set; }

    [field: SerializeField]
    public int AttackSFX { get; set; }

    [field: SerializeField]
    public int DropID { get; set; }

    public GameObject monsterPrefab;
}
public class MonsterTable : DataTable
{
    private Dictionary<int, MonsterData> monsterDataTable = new Dictionary<int, MonsterData>();
    private readonly string pathFormat = "Prefabs/Monster/{0}";
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var MonsterDataList = LoadCSV<MonsterData>(textAsset.text);

        monsterDataTable.Clear();

        foreach (var monster in MonsterDataList)
        {
            if (!monsterDataTable.ContainsKey(monster.MonsterID))
            {
                monsterDataTable.Add(monster.MonsterID, monster);
                monster.monsterPrefab = (GameObject)(Resources.Load(string.Format(pathFormat, monster.PrefabName), typeof(GameObject)));
                // item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.itemIconSpriteID), typeof(Sprite)));
            }
            else
            {
                Debug.LogError($"Key Duplicated {monster.MonsterID}");
            }
        }
    }
    public MonsterData Get(int key)
    {
        if (!monsterDataTable.ContainsKey(key))
        {
            return default;
        }

        return monsterDataTable[key];
    }
}

