using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public Dictionary<InteractType, List<GatherSaveInfo>> gatherSaveInfoTable = new Dictionary<InteractType, List<GatherSaveInfo>>();
    public List<MonsterSaveInfo> monsterSaveInfoList = new List<MonsterSaveInfo>();
    public List<Item> itemList = new List<Item>();

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV1();
        return data;
    }
}