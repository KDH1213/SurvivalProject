using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // "StringTableEn",
        // "StringTableJp",
    };
}

public static class ItemTableIds
{
    public static readonly string[] String =
    {
        "ItemTable",
    };
}

public static class WeaponTableIds
{
    public static readonly string[] String =
    {
        "WeaponTable",
    };
}
public static class GatherTableIds
{
    public static readonly string[] String =
    {
        "GatherTable",
    };
}

public static class ArmorTableIds
{
    public static readonly string[] String =
    {
        "ArmorTable",
    };
}

public static class WaveDataTableIds
{
    public static readonly string[] String =
    {
        "WaveTable",
    };
}
public static class MonsterTableIds
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

public static class PlacementTableIds
{
    public static readonly string ConstructionTable = "ConstructionTable";
    public static readonly string StructureTable = "StructureTable";
    public static readonly string ItemCreateTable = "ItemCreateTable";

}

public static class Varibalbes
{
    public static Languages currentLanguage = Languages.Korea;
}

public static class Tags
{
    public static readonly string Player = "Player";
    public static readonly string GameTimer = "GameTimer";
    public static readonly string GameController = "GameController";
    public static readonly string OverlapCollider = "OverlapCollider";
    public static readonly string StageManager = "StageManager";
    public static readonly string PointStructure = "PointStructure";
}

public static class SceneName
{
    public static readonly string Develop = "Assets/Scenes/Develop/Develop.unity";
    public static readonly string Stage1 = "Stage1";
    public static readonly string Stage2 = "Stage2";
    public static readonly string Stage3 = "Stage3";
}


public static class TypeName
{

    public static readonly string[] LifeSkillTypeName =
    {
        "공격력 증가",
        "이동속도 증가",
        "공격속도 증가",
        "배고픔 주기 감소",
        "갈증 주기 감소",
        "방어력 증가",
        "체력 증가",
        "피로 덜 느낌",
    };

}