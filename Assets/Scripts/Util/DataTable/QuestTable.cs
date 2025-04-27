
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    None,
    ItemCollection,
    ItemCrafting,
    MonsterHunting,
    Building,
    Movement,
}

public struct QuestDataInfo
{
    public QuestType questType;
    public int questTargetID;
    public int clearCount;
    public bool includeMine;

    public QuestDataInfo(QuestType questType, int questTargetID, int clearCount, bool includeMine)
    {
        this.questType = questType;
        this.questTargetID = questTargetID;
        this.clearCount = clearCount;
        this.includeMine = includeMine;
    }
}


[System.Serializable]
public class QuestData
{
    [field: SerializeField]
    public int QuestID { get; set; }
    [field: SerializeField]
    public int NameID { get; set; }
    [field: SerializeField]
    public int DescriptID { get; set; }
    [field: SerializeField]
    public int QuestNumber { get; set; }
    [field: SerializeField]
    public int QuestCategory { get; set; }
    [field: SerializeField]
    public int NextQuestID { get; set; }

    [field: SerializeField]
    public float QuestAreaX { get; set; }

    [field: SerializeField]
    public float QuestAreaZ { get; set; }

    [field: SerializeField]
    public float QuestAreaRadius { get; set; }
    [field: SerializeField]
    public int UseQuestMarkRangeInGame { get; set; }

    [field: SerializeField]
    public int UseMinimapIcon { get; set; }

    [field: SerializeField]
    public int QuestType1 { get; set; }

    [field: SerializeField]
    public int TargetID1 { get; set; }

    [field: SerializeField]
    public int ClearCount1 { get; set; }

    [field: SerializeField]
    public int IncludeMine1 { get; set; }

    [field: SerializeField]
    public int QuestType2 { get; set; }

    [field: SerializeField]
    public int TargetID2 { get; set; }

    [field: SerializeField]
    public int ClearCount2 { get; set; }

    [field: SerializeField]
    public int IncludeMine2 { get; set; }

    [field: SerializeField]
    public int QuestType3 { get; set; }

    [field: SerializeField]
    public int TargetID3 { get; set; }

    [field: SerializeField]
    public int ClearCount3 { get; set; }

    [field: SerializeField]
    public int IncludeMine3 { get; set; }


    [field: SerializeField]
    public int QuestType4 { get; set; }

    [field: SerializeField]
    public int TargetID4 { get; set; }

    [field: SerializeField]
    public int ClearCount4 { get; set; }

    [field: SerializeField]
    public int IncludeMine4 { get; set; }


    [field: SerializeField]
    public int DropExp { get; set; }

    [field: SerializeField]
    public int DropID { get; set; }

    public List<QuestDataInfo> questInfoList = new List<QuestDataInfo>();
    public Vector3 GetTargetPosition() { return new Vector3(QuestAreaX, 0f, QuestAreaZ); }

    public void Initialize()
    {
        if(QuestType1 == 0)
        {
            return;
        }
        questInfoList.Add(new QuestDataInfo((QuestType)QuestType1, TargetID1, ClearCount1, IncludeMine1 != 0));

        if (QuestType2 == 0)
        {
            return;
        }
        questInfoList.Add(new QuestDataInfo((QuestType)QuestType2, TargetID2, ClearCount2, IncludeMine2 != 0));

        if (QuestType3 == 0)
        {
            return;
        }
        questInfoList.Add(new QuestDataInfo((QuestType)QuestType3, TargetID3, ClearCount3, IncludeMine3 != 0));

        if (QuestType4 == 0)
        {
            return;
        }
        questInfoList.Add(new QuestDataInfo((QuestType)QuestType4, TargetID4, ClearCount4, IncludeMine4 != 0));
    }
}
public class QuestTable : DataTable
{
    private Dictionary<int, QuestData> questDataTable = new Dictionary<int, QuestData>();
    private List<QuestData> questList = new List<QuestData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        questList = LoadCSV<QuestData>(textAsset.text);

        questDataTable.Clear();

        foreach (var questData in questList)
        {
            if (!questDataTable.ContainsKey(questData.QuestID))
            {
                questDataTable.Add(questData.QuestID, questData);
                questData.Initialize();
            }
            else
            {
                Debug.LogError($"Key Duplicated {questData.QuestID}");
            }
        }
    }

    public QuestData Get(int key)
    {
        if (!questDataTable.ContainsKey(key))
        {
            return default;
        }

        return questDataTable[key];
    }
}