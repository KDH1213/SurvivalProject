using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "PlayerStat/StatData")]
public class PlayerStatData : ScriptableObject
{
    public int Hp;
    public float Speed;
    public float AttackRange;
    public float DefaultDamage;
}
