using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureStats : CharactorStats, ISaveLoadData
{
    // TODO :: 테스트 코드
    [SerializeField]
    private float hp;

    public float HP { get { return currentStatTable[StatType.HP].Value; } }

    protected override void Awake()
    {
    }

    public void Init()
    {
        currentStatTable.Clear();
        currentStatTable.Add(StatType.HP, new StatValue(StatType.HP, hp));
    }

    public void Load()
    {
    }

    public void Save()
    {
    }
}
