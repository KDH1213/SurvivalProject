using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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
    private int currentMonsterQuestCount = 0;
    private int activeQuestCount = 0;

    private Transform playerTransform;
    private Vector2 targetPosition;
    private float distance = 0f;

    private void Awake()
    {
        if(currentQuestID == 0)
        {
            return;
        }

        Initialized();
        currentQuestData = DataTableManager.QuestTable.Get(currentQuestID);
        StartQuest(currentQuestData);
        playerTransform = GameObject.FindWithTag(Tags.Player).transform;

        onChangeValueEvent.AddListener(questView.OnSetTargetCount);
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
        currentMonsterQuestCount = 0;
        activeQuestCount = 0;

        for (int i = 0; i < questCount; ++i)
        {
            switch (questInfoList[i].questType)
            {
                case QuestType.None:
                    break;
                case QuestType.ItemCollection:
                    break;
                case QuestType.ItemCrafting:
                    break;
                case QuestType.MonsterHunting:
                    questProgressInfoList[i].SetInfo(i, questInfoList[i].questTargetID, 0, questInfoList[i].clearCount);
                    var questProgressInfo = questProgressInfoList[i];
                    currentMonsterQuestList.Add(questProgressInfo);
                    ++currentMonsterQuestCount;

                    onChangeValueEvent?.Invoke(i, questProgressInfo.currentCount, questProgressInfo.maxCount);
                    break;
                case QuestType.Building:
                    break;
                case QuestType.Movement:
                    targetPosition = questData.GetTargetPosition().ConvertVector2();
                    distance = questData.QuestAreaRadius;
                    enabled = true;

                    break;
                default:
                    break;
            }

            ++activeQuestCount;
        }
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

    private void CheckQuestClear()
    {
        if (currentQuestData.questInfoList[0].questType == QuestType.Movement)
        {
            SetNextQuest();
            return;
        }

        if(activeQuestCount == 0)
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

    public void Load()
    {
    }

    public void Save()
    {
    }
}
