using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestProgressInfo
{
    public int index;
    public int targetID;
    public int currentCount;
    public int maxCount;

    public void SetInfo(int index, int targetId, int currentCount, int maxCount)
    {
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

public class QuestSystem : MonoBehaviour, ISaveLoadData
{
    private int currentQuestID = 1000;
    private QuestData currentQuestData;

    [SerializeField]
    private UIQuestView questView;

    public UnityEvent<int, int, int> onChangeValueEvent;

    private List<QuestProgressInfo> questProgressInfoList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentMonsterQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentItemCollectionQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentCreateItemQuestList = new List<QuestProgressInfo>();
    private List<QuestProgressInfo> currentBulidingQuestList = new List<QuestProgressInfo>();

    private int currentCreatItemQuestCount = 0;
    private int currentMonsterQuestCount = 0;
    private int currentItemCollectionQuestCount = 0;
    private int currentBulidingQuestCount = 0;
    private int activeQuestCount = 0;

    private Transform playerTransform;
    private Vector2 targetPosition;
    private float distance = 0f;

    private PlacementSystem placementSystem;
    private MiniMapController miniMapController;

    private UnityAction onIconDisableAction;

    private void Awake()
    {
        if(currentQuestID == 0)
        {
            return;
        }

        Initialized();
        currentQuestData = DataTableManager.QuestTable.Get(currentQuestID);
      
        playerTransform = GameObject.FindWithTag(Tags.Player).transform;

        var placementSystemObject = GameObject.FindWithTag(Tags.PlacementSystem);
        if(placementSystemObject != null)
        {
            placementSystem =  placementSystemObject.GetComponent<PlacementSystem>();
            placementSystem.onChangeBuildingCountEvnet.AddListener(OnCreateBuilding);
        }

        onChangeValueEvent.AddListener(questView.OnSetTargetCount);
        questView.SetClickCompensationAction(OnCompensation);

        miniMapController = GameObject.FindWithTag("MiniMap").GetComponent<MiniMapController>();
    }

    private void Start()
    {
        if(currentQuestData != null)
        {
            StartQuest(currentQuestData);
        }
    }

    private void Update()
    {
        var currentPosition = playerTransform.position.ConvertVector2();
        if((currentPosition - targetPosition).magnitude < distance)
        {
            enabled = false;
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

            if (questInfoList[i].questType != QuestType.Movement)
            {
                questProgressInfoList[i].SetInfo(i, questInfoList[i].questTargetID, 0, questInfoList[i].clearCount);
                onChangeValueEvent?.Invoke(i, questProgressInfo.currentCount, questProgressInfo.maxCount);
            }
            ++activeQuestCount;

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

    public void OnAddItem(int itemID, int addItemCount)
    {
        if (currentItemCollectionQuestCount == 0)
        {
            return;
        }

        for (int i = 0; i < currentItemCollectionQuestCount; ++i)
        {
            if (currentItemCollectionQuestList[i].targetID == itemID)
            {
                currentItemCollectionQuestList[i].currentCount += addItemCount;
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

    private void CheckQuestClear()
    {
        onIconDisableAction?.Invoke();
        questView.OnActiveButtonView();
    }

    private void OnCompensation()
    {
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
        if(currentQuestData.NextQuestID == 0)
        {
            questView.CloseQuestView();
            return;
        }

        currentQuestData = DataTableManager.QuestTable.Get(currentQuestData.NextQuestID);
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
    }

    public void Save()
    {
    }
}
