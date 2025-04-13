using System.Collections.Generic;


public class PlayerSaveData
{
    public int equipmentConsumableCount = 1;
    public List<ItemInfoSaveData> ItemSlotInfoSaveDataList = new List<ItemInfoSaveData>();
    public List<int> skillUiViewSeleteList = new List<int>();
    public List<int> equipmentItemIDList = new List<int>();
    public LevelStatInfo levelStatInfo = new LevelStatInfo();
    public PlayerSaveInfo playerSaveInfo = new PlayerSaveInfo();
    public System.DateTime gameTime = new System.DateTime();
    public int playStage = 0;

    public PlayerSaveData()
    {
        playerSaveInfo.survivalStatValues = new float[(int)SurvivalStatType.End];

        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Hunger] = 10000f;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Thirst] = 10000f;
        levelStatInfo.skillLevelList = new List<int>();

        playerSaveInfo.hp = 100;
    }
}

public class StageSaveData
{
    public List<PlacementSaveInfo> placementSaveInfoList = new List<PlacementSaveInfo>();
    public List<FarmPlacementSaveInfo> farmPlacementSaveInfos = new List<FarmPlacementSaveInfo>();
    public Dictionary<InteractType, List<GatherSaveInfo>> gatherSaveInfoTable = new Dictionary<InteractType, List<GatherSaveInfo>>();
    public List<MonsterSaveInfo> monsterSaveInfoList = new List<MonsterSaveInfo>();
    public MonsterWaveSaveInfo monsterWaveSaveInfo = new MonsterWaveSaveInfo();
    public UnityEngine.Vector3 playerPosition;

    public StageSaveData() 
    {
    }
}


public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public StageSaveData[] stageSaveDatas = new StageSaveData[7];
    public PlayerSaveData PlayerSaveData = new PlayerSaveData();
    public int playStage = 0;
    public StageSaveData StageSaveData { get { return stageSaveDatas[6]; } }

    public SaveDataV1()
    {
        Version = 1;

        for(int i= 0; i < 7; ++i)
        {
            stageSaveDatas[i] = new StageSaveData();
        }
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV1();

        return data;
    }
}