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
    public List<WaveMonsterSaveInfo> waveMonsterSaveInfos = new List<WaveMonsterSaveInfo>();
    public List<SpawnerSaveInfo> spawnerSaveInfoList = new List<SpawnerSaveInfo>();
    public MonsterWaveSaveInfo monsterWaveSaveInfo = new MonsterWaveSaveInfo();

    public List<int> equipmentItemIDList = new List<int>();
    public int equipmentConsumableCount = 1;
    public List<int> skillUiViewSeleteList = new List<int>();
    public bool isSkillReroll = true;


    public LevelStatInfo levelStatInfo = new LevelStatInfo();
    public PlayerSaveInfo playerSaveInfo = new PlayerSaveInfo();
    public System.DateTime gameTime = new System.DateTime();

    public bool isRestart = false;
    public SaveDataV1()
    {
        Version = 1;
        playerSaveInfo.survivalStatValues = new float[(int)SurvivalStatType.End];
        levelStatInfo.skillLevelList = new List<int>();

        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Hunger] = 10000;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Thirst] = 10000;
        playerSaveInfo.hp = 100f;
    }

    public override SaveData VersionUp()
    {
        var data = new SaveDataV1();
        return data;
    }

    public void ResetStageInfo()
    {
        placementSaveInfoList.Clear();
        farmPlacementSaveInfos.Clear();
        gatherSaveInfoTable.Clear();
        monsterSaveInfoList.Clear();
        ItemSlotInfoSaveDataList.Clear();
        waveMonsterSaveInfos.Clear();
        spawnerSaveInfoList.Clear();
        equipmentItemIDList.Clear();

        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Hunger] = 10000;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Thirst] = 10000;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Fatigue] = 0;

        isRestart = true;

        // monsterWaveSaveInfo;
    }
}