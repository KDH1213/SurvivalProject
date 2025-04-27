using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructedMonsterEvent : MonoBehaviour, IDestructible
{
    private int monsterID;
    public UnityEvent<int> onMonsterDeathEvent;

    private void Awake()
    {
        var questSystem = GameObject.FindWithTag(Tags.QuestSystem);

        if(questSystem != null)
        {
            onMonsterDeathEvent.AddListener(questSystem.GetComponent<QuestSystem>().OnDeathMonster);
        }
    }

    private void Start()
    {
        monsterID = GetComponent<MonsterFSM>().MonsterData.MonsterID;
    }

    public void OnDestruction(GameObject attacker)
    {
        onMonsterDeathEvent?.Invoke(monsterID);
    }
}
