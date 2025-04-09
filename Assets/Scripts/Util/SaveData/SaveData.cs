using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public List<PlacementSaveInfo> placementSaveInfoList = new List<PlacementSaveInfo>();
    public List<FarmPlacementSaveInfo> farmPlacementSaveInfos = new List<FarmPlacementSaveInfo>();
    public Dictionary<InteractType, List<GatherSaveInfo>> gatherSaveInfoTable = new Dictionary<InteractType, List<GatherSaveInfo>>();
    public List<MonsterSaveInfo> monsterSaveInfoList = new List<MonsterSaveInfo>();
    public List<ItemInfoSaveData> ItemSlotInfoSaveDataList = new List<ItemInfoSaveData>();

    public List<int> equipmentItemIDList = new List<int>();
    public PlayerSaveInfo playerSaveInfo = new PlayerSaveInfo();
    public System.DateTime gameTime = new System.DateTime();

    public SaveDataV1()
    {
        Version = 1;
        playerSaveInfo.survivalStatValues = new float[(int)SurvivalStatType.End];
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV1();
        return data;
    }
}