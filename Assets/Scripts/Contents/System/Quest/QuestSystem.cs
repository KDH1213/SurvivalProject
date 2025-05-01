using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestProgressInfo
{
    public QuestType questType;
    public int index;
    public int targetID;
    public int currentCount;
    public int maxCount;

    public void SetInfo(QuestType questType, int index, int targetId, int currentCount, int maxCount)
    {
        this.questType = questType;
        this.index = index;
        this.targetID = targetId;
        this.currentCount = currentCount;
        this.maxCount = maxCount;
    }

    public void AddCount()
    {
        ++currentCount;
    }

    public bool IsClear()
    {
        return (currentCount == maxCount);
    }
}

[System.Serializable]
public struct QuestProgressSaveInfo
{
    public int questID;
    public List<QuestProgressInfo> questProgressInfoList;
}

public class QuestSystem : MonoBehaviour, ISaveLoadData
{
    private int currentQuestID = 1000;
    private QuestData currentQuestData;

    [SerializeField]
    private UIQuestView questView;

    public UnityEvent<int, int, int> onChangeValueEvent;

    private List<QuestProgressInfo> questProgressInfoList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> tempQuestClearList = new List<QuestProgressInfo>();

    private List<QuestProgressInfo> currentMonsterQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentItemCollectionQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentCreateItemQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentBulidingQuestList = new List<QuestProgressInfo>();

    private List<QuestProgressInfo> currentUseItemQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentDestructMonsterStrongQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentRelicsCollectionQuestList = new List<QuestProgressInfo>();

    // private Dictionary<QuestType, List<QuestProgressInfo>> questProgressInfoTable = new Dictionary<QuestType, List<QuestProgressInfo>>();

    private int currentCreatItemQuestCount = 0;
    private int currentMonsterQuestCount = 0;
    private int currentItemCollectionQuestCount = 0;
    private int currentBulidingQuestCount = 0;

    private int currentUseItemQuestCount = 0;
    private int currentDestructMonsterStrongQuestCount = 0;
    private int currentRelicsCollectionQuestCount = 0;
    private int activeQuestCount = 0;

    private Transform playerTransform;
    private Vector2 targetPosition;
    private float distance = 0f;

    private PlacementSystem placementSystem;
    private MiniMapController miniMapController;

    private UnityEvent<DropItemInfo> onQuestCompensationEvent = new();
    private UnityAction onIconDisableAction;

    private bool isLoad = false;

    private void Awake()
    {
        if(currentQuestID == 0)
        {
            return;
        }

        var stageManager = GameObject.FindWithTag("StageManager");
        if (stageManager != null)
        {
            stageManager.GetComponent<StageManager>().onSaveEvent += Save;
        }


        Initialized();
        // currentQuestData = DataTableManager.QuestTable.Get(currentQuestID);      
        playerTransform = GameObject.FindWithTag(Tags.Player).transform;

        var playerFsm = playerTransform.GetComponent<PlayerFSM>();
        playerFsm.PlayerInventory.onUseItemEvent.AddListener(OnUseItem);
        onQuestCompensationEvent.AddListener(playerFsm.OnDropItem);

        var placementSystemObject = GameObject.FindWithTag(Tags.PlacementSystem);
        if(placementSystemObject != null)
        {
            placementSystem =  placementSystemObject.GetComponent<PlacementSystem>();
            placementSystem.onChangeBuildingCountEvnet.AddListener(OnCreateBuilding);
        }

        onChangeValueEvent.AddListener(questView.OnSetTargetCount);
        questView.SetClickCompensationAction(OnCompensation);

        miniMapController = GameObject.FindWithTag("MiniMap").GetComponent<MiniMapController>();

        var monsterSpawnSystem = GameObject.FindWithTag(Tags.MonsterSpawnSystem);
        if(monsterSpawnSystem != null)
        {
            monsterSpawnSystem.GetComponent<MonsterSpawnSystem>().onDestorySpawnerEvent.AddListener(OnDestroyMonsterStrongpoint);
        }
    }

    private void Start()
    {
        if (SaveLoadManager.Data != null)
        {
            Load();
        }
        if (currentQuestID != 0 && !isLoad)
        {
            currentQuestData = DataTableManager.QuestTable.Get(currentQuestID);
            StartQuest(currentQuestData);
        }
    }

    private void Update()
    {
        var currentPosition = playerTransform.position.ConvertVector2();
        if((currentPosition - targetPosition).magnitude < distance)
        {
            enabled = false;
            --activeQuestCount;
            CheckQuestClear();
        }
    }

    public void StartQuest(QuestData questData)
    {
        var questInfoList = questData.questInfoList;
        var questCount = questInfoList.Count;

        questView.SetQuestInfo(questData);
        currentMonsterQuestList.Clear();
        currentCreateItemQuestList.Clear();

        currentItemCollectionQuestCount = 0;
        currentMonsterQuestCount = 0;
        currentCreatItemQuestCount = 0;
        activeQuestCount = 0;

        for (int i = 0; i < questCount; ++i)
        {
            var questProgressInfo = questProgressInfoList[i];

            ++activeQuestCount;

            if (questInfoList[i].questType != QuestType.Movement)
            {
                questProgressInfoList[i].SetInfo(questInfoList[i].questType, i, questInfoList[i].questTargetID, 0, questInfoList[i].clearCount);
                onChangeValueEvent?.Invoke(i, questProgressInfo.currentCount, questProgressInfo.maxCount);
            }
            else
            {
                questProgressInfoList[i].SetInfo(questInfoList[i].questType, i, questInfoList[i].questTargetID, -1, questInfoList[i].clearCount);
            }

            switch (questInfoList[i].questType)
            {
                case QuestType.None:
                    break;
                case QuestType.ItemCollection:
                    ++currentItemCollectionQuestCount;
                    currentItemCollectionQuestList.Add(questProgressInfo);
                    break;
                case QuestType.ItemCrafting:
                    currentCreateItemQuestList.Add(questProgressInfo);
                    ++currentCreatItemQuestCount;
                    break;
                case QuestType.MonsterHunting:
                    currentMonsterQuestList.Add(questProgressInfo);
                    ++currentMonsterQuestCount;
                    break;
                case QuestType.Building:
                    currentBulidingQuestList.Add(questProgressInfo);
                    ++currentBulidingQuestCount;

                    if(questInfoList[i].includeMine)
                    {
                        if(placementSystem.PlacedGameObjectTable.TryGetValue(questProgressInfo.targetID, out var count))
                        {
                            OnCreateBuilding(questProgressInfo.targetID, count);
                        }
                    }
                    break;
                case QuestType.Movement:
                    targetPosition = questData.GetTargetPosition().ConvertVector2();
                    distance = questData.QuestAreaRadius;
                    enabled = true;
                    break;
                case QuestType.UseItem:
                    currentUseItemQuestList.Add(questProgressInfo);
                    ++currentUseItemQuestCount;
                    break;

                case QuestType.DestructMonsterStrong:
                    currentDestructMonsterStrongQuestList.Add(questProgressInfo);
                    ++currentDestructMonsterStrongQuestCount;

                    if (questInfoList[i].includeMine)
                    {
                        var monsterSpawnSystem = GameObject.FindWithTag(Tags.MonsterSpawnSystem).GetComponent<MonsterSpawnSystem>();
                        OnDestroyMonsterStrongpoint(monsterSpawnSystem.ActiveSpwanerCount, monsterSpawnSystem.TotalSpawnerCount);
                    }
                    break;
                case QuestType.RelicsCollection:
                    currentRelicsCollectionQuestList.Add(questProgressInfo);
                    ++currentRelicsCollectionQuestCount;
                    break;
                default:
                    break;
            }
        }

        miniMapController.AddQuestObject(currentQuestData, ref onIconDisableAction);
    }

    public void OnDeathMonster(int monsterID)
    {
        if (currentMonsterQuestCount == 0)
        {
            return; 
        }

        for (int i = 0; i < currentMonsterQuestCount; ++i)
        {
            if (currentMonsterQuestList[i].targetID == monsterID)
            {
                currentMonsterQuestList[i].AddCount();
                onChangeValueEvent?.Invoke(currentMonsterQuestList[i].index, currentMonsterQuestList[i].currentCount, currentMonsterQuestList[i].maxCount);
                if(currentMonsterQuestList[i].IsClear())
                {
                    currentMonsterQuestList.Remove(currentMonsterQuestList[i]);
                    --currentMonsterQuestCount;
                    --activeQuestCount;
                    CheckQuestClear();
                }
                break;
            }
        }
    }

    public void OnAddItem(DropItemInfo dropItemInfo)
    {
        if (currentItemCollectionQuestCount == 0)
        {
            return;
        }

        for (int i = 0; i < currentItemCollectionQuestCount; ++i)
        {
            if (currentItemCollectionQuestList[i].targetID == dropItemInfo.id)
            {
                currentItemCollectionQuestList[i].currentCount += dropItemInfo.amount;
                onChangeValueEvent?.Invoke(currentItemCollectionQuestList[i].index, currentItemCollectionQuestList[i].currentCount, currentItemCollectionQuestList[i].maxCount);
                if (currentItemCollectionQuestList[i].IsClear())
                {
                    currentItemCollectionQuestList.Remove(currentItemCollectionQuestList[i]);
                    --currentItemCollectionQuestCount;
                    --activeQuestCount;
                    CheckQuestClear();
                }
                break;
            }
        }
    }

    public void OnCreateBuilding(int buildID, int buildCount)
    {
        if (currentBulidingQuestCount == 0)
        {
            return;
        }

        for (int i = 0; i < currentBulidingQuestCount; ++i)
        {
            if (currentBulidingQuestList[i].targetID == buildID)
            {
                currentBulidingQuestList[i].currentCount = buildCount;
                onChangeValueEvent?.Invoke(currentBulidingQuestList[i].index, currentBulidingQuestList[i].currentCount, currentBulidingQuestList[i].maxCount);
                if (currentBulidingQuestList[i].IsClear())
                {
                    currentBulidingQuestList.Remove(currentBulidingQuestList[i]);
                    --currentBulidingQuestCount;
                    --activeQuestCount;
                    CheckQuestClear();
                }
                break;
            }
        }
    }

    public void OnCreateItem(int itemID, int createCount)
    {
        if (currentCreatItemQuestCount == 0)
        {
            return;
        }

        for (int i = 0; i < currentCreatItemQuestCount; ++i)
        {
            if (currentCreateItemQuestList[i].targetID == itemID)
            {
                currentCreateItemQuestList[i].currentCount += createCount;
                onChangeValueEvent?.Invoke(currentCreateItemQuestList[i].index, currentCreateItemQuestList[i].currentCount, currentCreateItemQuestList[i].maxCount);
                if (currentCreateItemQuestList[i].IsClear())
                {
                    currentCreateItemQuestList.Remove(currentCreateItemQuestList[i]);
                    --currentCreatItemQuestCount;
                    --activeQuestCount;
                    CheckQuestClear();
                }
                break;
            }
        }
    }

    public void OnUseItem(int itemID)
    {
        if(currentUseItemQuestCount == 0)
        {
            return;
        }

        for (int i = 0; i < currentUseItemQuestCount; ++i)
        {
            if (currentUseItemQuestList[i].targetID == itemID)
            {
                currentUseItemQuestList[i].currentCount += 1;
                onChangeValueEvent?.Invoke(currentUseItemQuestList[i].index, currentUseItemQuestList[i].currentCount, currentUseItemQuestList[i].maxCount);
                if (currentUseItemQuestList[i].IsClear())
                {
                    currentUseItemQuestList.Remove(currentUseItemQuestList[i]);
                    --currentUseItemQuestCount;
                    --activeQuestCount;
                    CheckQuestClear();
                }
                break;
            }
        }
    }

    public void OnDestroyMonsterStrongpoint(int destroyCount, int maxCount)
    {
        if(currentDestructMonsterStrongQuestCount == 0)
        {
            return;
        }

        for (int i = 0; i < currentDestructMonsterStrongQuestCount; ++i)
        {
            currentDestructMonsterStrongQuestList[i].currentCount = destroyCount;
            onChangeValueEvent?.Invoke(currentDestructMonsterStrongQuestList[i].index, currentDestructMonsterStrongQuestList[i].currentCount, currentDestructMonsterStrongQuestList[i].maxCount);
            if (currentDestructMonsterStrongQuestList[i].IsClear())
            {
                currentDestructMonsterStrongQuestList.Remove(currentDestructMonsterStrongQuestList[i]);
                --currentDestructMonsterStrongQuestCount;
                --activeQuestCount;
                CheckQuestClear();
            }
            break;
        }
    }

    private void OnCheckQuestClear(ref List<QuestProgressInfo> questProgressInfoList, ref int questCount)
    {
        for (int i = 0; i < questProgressInfoList.Count; ++i)
        {
            if (questProgressInfoList[i].IsClear())
            {
                tempQuestClearList.Add(questProgressInfoList[i]);
                --questCount;
                --activeQuestCount;
                CheckQuestClear();
            }
        }

        foreach (var questClear in tempQuestClearList)
        {
            questProgressInfoList.Remove(questClear);
        }

        tempQuestClearList.Clear();
    }


    public void OnCollectionRelics(int collectionCount)
    {

    }

    private void CheckQuestClear()
    {
        if(activeQuestCount == 0)
        {
            tempQuestClearList.Clear();
            onIconDisableAction?.Invoke();
            questView.OnActiveButtonView();
        }
    }

    private void OnCompensation()
    {
        //var dropItemList = DataTableManager.DropTable.Get(currentQuestData.DropID).GetDropItemList();

        //foreach (var dropItem in dropItemList)
        //{
        //    onQuestCompensationEvent?.Invoke(dropItem);
        //}

        if (currentQuestData.questInfoList[0].questType == QuestType.Movement)
        {
            SetNextQuest();
            return;
        }

        if (activeQuestCount == 0)
        {
            SetNextQuest();
        }
    }

    private void SetNextQuest()
    {

        if (currentQuestData.NextQuestID == 0)
        {
            questView.CloseQuestView();
            return;
        }

        currentQuestID = currentQuestData.NextQuestID;
        currentQuestData = DataTableManager.QuestTable.Get(currentQuestID);
        StartQuest(currentQuestData);
    }

    private void Initialized()
    {
        for (int i = 0; i < 4; ++i)
        {
            questProgressInfoList.Add(new QuestProgressInfo());
        }
    }

    public void SetPlacementSystem(PlacementSystem placementSystem)
    {
        this.placementSystem = placementSystem;
    }
    public void Load()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        var questProgressSaveInfo = SaveLoadManager.Data.quesetProgressSaveInfo;
        currentQuestID = questProgressSaveInfo.questID;

        if(questProgressSaveInfo.questProgressInfoList.Count == 0 || currentQuestID == 0)
        {
            return;
        }

        currentQuestData = DataTableManager.QuestTable.Get(currentQuestID);

        questView.SetQuestInfo(currentQuestData);
        currentMonsterQuestList.Clear();
        currentCreateItemQuestList.Clear();

        currentItemCollectionQuestCount = 0;
        currentMonsterQuestCount = 0;
        currentCreatItemQuestCount = 0;
        activeQuestCount = 0;

        var questSaveProgressInfoList = questProgressSaveInfo.questProgressInfoList;
        int count = questSaveProgressInfoList.Count;

        for (int i = 0; i < count; ++i)
        {
            var questProgressInfo = questSaveProgressInfoList[i];

            ++activeQuestCount;
            if (questSaveProgressInfoList[i].questType != QuestType.Movement)
            {
                questProgressInfoList[i].SetInfo(questSaveProgressInfoList[i].questType, i, questSaveProgressInfoList[i].targetID, questSaveProgressInfoList[i].currentCount, questSaveProgressInfoList[i].maxCount);
                onChangeValueEvent?.Invoke(i, questProgressInfo.currentCount, questProgressInfo.maxCount);
            }
            else
            {
                questProgressInfoList[i].SetInfo(questSaveProgressInfoList[i].questType, i, questSaveProgressInfoList[i].targetID, -1, questSaveProgressInfoList[i].maxCount);
            }

            if(!questProgressInfoList[i].IsClear())
            {
                switch (questSaveProgressInfoList[i].questType)
                {
                    case QuestType.None:
                        break;
                    case QuestType.ItemCollection:
                        ++currentItemCollectionQuestCount;
                        currentItemCollectionQuestList.Add(questProgressInfo);
                        break;
                    case QuestType.ItemCrafting:
                        currentCreateItemQuestList.Add(questProgressInfo);
                        ++currentCreatItemQuestCount;
                        break;
                    case QuestType.MonsterHunting:
                        currentMonsterQuestList.Add(questProgressInfo);
                        ++currentMonsterQuestCount;
                        break;
                    case QuestType.Building:
                        currentBulidingQuestList.Add(questProgressInfo);
                        ++currentBulidingQuestCount;
                        break;
                    case QuestType.Movement:
                        targetPosition = currentQuestData.GetTargetPosition().ConvertVector2();
                        distance = currentQuestData.QuestAreaRadius;
                        enabled = true;
                        break;
                    case QuestType.UseItem:
                        currentUseItemQuestList.Add(questProgressInfo);
                        ++currentUseItemQuestCount;
                        break;

                    case QuestType.DestructMonsterStrong:
                        currentDestructMonsterStrongQuestList.Add(questProgressInfo);
                        ++currentDestructMonsterStrongQuestCount;
                        break;
                    case QuestType.RelicsCollection:
                        currentRelicsCollectionQuestList.Add(questProgressInfo);
                        ++currentRelicsCollectionQuestCount;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                --activeQuestCount;
            }
        }

        CheckQuestClear();
        miniMapController.AddQuestObject(currentQuestData, ref onIconDisableAction);
        isLoad = true;
    }

    public void Save()
    {
        var quesetProgressSaveInfo = new QuestProgressSaveInfo();
        quesetProgressSaveInfo.questProgressInfoList = new List<QuestProgressInfo>();
        quesetProgressSaveInfo.questID = currentQuestID;

        int questCount = DataTableManager.QuestTable.Get(quesetProgressSaveInfo.questID).questInfoList.Count;

        for (int i = 0; i < questCount; ++i)
        {
            quesetProgressSaveInfo.questProgressInfoList.Add(questProgressInfoList[i]);
        }

        SaveLoadManager.Data.quesetProgressSaveInfo = quesetProgressSaveInfo;   
    }
}
