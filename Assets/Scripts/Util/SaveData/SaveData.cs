using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public List<PlacementSaveInfo> placementSaveInfoList = new List<PlacementSaveInfo>();
    public List<FarmPlacementSaveInfo> farmPlacementSaveInfos = new List<FarmPlacementSaveInfo>();
    public List<StoragePlacementSaveInfo> storagePlacementSaveInfo = new List<StoragePlacementSaveInfo>();
    public Dictionary<InteractType, List<GatherSaveInfo>> gatherSaveInfoTable = new Dictionary<InteractType, List<GatherSaveInfo>>();
    public List<MonsterSaveInfo> monsterSaveInfoList = new List<MonsterSaveInfo>();
    public List<ItemInfoSaveData> ItemSlotInfoSaveDataList = new List<ItemInfoSaveData>();
    public List<WaveMonsterSaveInfo> waveMonsterSaveInfos = new List<WaveMonsterSaveInfo>();
    public List<SpawnerSaveInfo> spawnerSaveInfoList = new List<SpawnerSaveInfo>();
    public MonsterWaveSaveInfo monsterWaveSaveInfo = new MonsterWaveSaveInfo();
    public BasePointerSaveInfo basePointerSaveInfo = new BasePointerSaveInfo();
    public List<EliteMonsterSaveInfo> eliteMonsterSaveInfoList = new List<EliteMonsterSaveInfo>();

    public List<int> equipmentItemIDList = new List<int>();
    public List<EquipmentItemInfoSaveData> equipmentItemInfoSaveDataList = new List<EquipmentItemInfoSaveData>();
    public int equipmentConsumableCount = 1;
    public List<int> skillUiViewSeleteList = new List<int>();
    public bool isSkillReroll = true;


    public LevelStatInfo levelStatInfo = new LevelStatInfo();
    public PlayerSaveInfo playerSaveInfo = new PlayerSaveInfo();
    public System.DateTime gameTime = new System.DateTime();
    public QuestProgressSaveInfo quesetProgressSaveInfo = new QuestProgressSaveInfo();

    public bool isRestart = false;
    public int stageTemperature = 0;

    public string startStage = SceneName.Stage1;
    public int sceneStage = SceneStage.Stage1;
    public SaveDataV1()
    {
        Version = 1;
        playerSaveInfo.survivalStatValues = new float[(int)SurvivalStatType.End];
        levelStatInfo.skillLevelList = new List<int>();

        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Hunger] = 10000;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Thirst] = 10000;
        playerSaveInfo.hp = 100f;
        quesetProgressSaveInfo.questID = 240011;
        quesetProgressSaveInfo.questProgressInfoList = new List<QuestProgressInfo>();

        var stageManager = GameObject.FindWithTag(Tags.StageManager);
        if(stageManager != null)
        {
            playerSaveInfo.position = stageManager.GetComponent<StageManager>().StartPosition;
        }
        else
        {
            playerSaveInfo.position = Vector3.right * 5f;
        }
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
        storagePlacementSaveInfo.Clear();
        gatherSaveInfoTable.Clear();
        monsterSaveInfoList.Clear();
        ItemSlotInfoSaveDataList.Clear();
        waveMonsterSaveInfos.Clear();
        spawnerSaveInfoList.Clear();
        equipmentItemIDList.Clear();
        eliteMonsterSaveInfoList.Clear();

        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Hunger] = 10000;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Thirst] = 10000;
        playerSaveInfo.survivalStatValues[(int)SurvivalStatType.Fatigue] = 0;
        playerSaveInfo.position = Vector3.left * 3f;

        stageTemperature = 0;
        isRestart = true;

        // monsterWaveSaveInfo;
    }

    public void NextStageInitiailze()
    {
        placementSaveInfoList.Clear();
        farmPlacementSaveInfos.Clear();
        storagePlacementSaveInfo.Clear();
        gatherSaveInfoTable.Clear();
        monsterSaveInfoList.Clear();
        waveMonsterSaveInfos.Clear();
        spawnerSaveInfoList.Clear();
        eliteMonsterSaveInfoList.Clear();
        monsterWaveSaveInfo = new MonsterWaveSaveInfo();
        basePointerSaveInfo = null;
        stageTemperature = 0;
        playerSaveInfo.position = Vector3.left * 3f;
    }
}