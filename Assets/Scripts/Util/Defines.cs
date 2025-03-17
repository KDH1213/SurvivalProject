using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Korea,
    English,
    Japanese,
}

public static class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableKr",
        "StringTableEn",
        "StringTableJp",
    };
}

public static class ItemTableIds
{
    public static readonly string[] String =
    {
        "ItemTable",
    };
}

public static class WaveDataTableIds
{
    public static readonly string[] String =
    {
        "WaveTable",
    };
}
public static class MonsterDataTableIds
{
    public static readonly string[] String =
    {
        "MonsterTable",
    };
}
public static class CoinDrawTableIds
{
    public static readonly string[] String =
    {
        "CoinDrawTable",
    };
}

public static class CharactorTableIds
{
    public static readonly string[] String =
    {
        "CharactorTable",
    };
}
public static class AttackTableIds
{
    public static readonly string[] String =
    {
        "AttackTable",
    };
}
public static class CharactorSellTableIds
{
    public static readonly string[] String =
    {
        "CharactorSellTable",
    };
}

public static class CombinationTableIds
{
    public static readonly string[] String =
    {
        "CombinationTable",
    };
}

public static class ReinforcedTableIds
{
    public static readonly string[] String =
    {
        "ReinforcedTable",
    };
}

public static class Varibalbes
{
    public static Languages currentLanguage = Languages.Korea;
}

public static class Tags
{
    public static readonly string Player = "Player";
    public static readonly string GameController = "GameController";
    public static readonly string OverlapCollider = "OverlapCollider";
    public static readonly string ReinforcedManager = "ReinforcedManager";
    public static readonly string DamagedObjectPool = "DamagedObjectPool";
}


public static class TypeStringTable
{

    public static readonly string[] DebuffTypeStrings =
    {
        "없음",
        "스턴",
        "느려짐"
    };

    public static readonly string[] AttackTypeStrings =
 {
        "단일 공격",
        "다중 공격",
        "범위 공격"
    };

}

public static class SortingLayer
{
    public static readonly string Defalyle = "Default";
}
