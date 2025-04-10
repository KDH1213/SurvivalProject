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

public static class WeaponTableIds
{
    public static readonly string[] String =
    {
        "WeaponTable",
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

public static class PlacementTableIds
{
    public static readonly string ConstructionTable = "ConstructionTable";
    public static readonly string FarmTable = "FarmTable";
    public static readonly string TurretTable = "TurretTable";
    public static readonly string OtherTable = "OtherTable";

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
}


//public static class TypeStringTable
//{

//    public static readonly string[] DebuffTypeStrings =
//    {
//        "����",
//        "����",
//        "������"
//    };

//    public static readonly string[] AttackTypeStrings =
// {
//        "���� ����",
//        "���� ����",
//        "���� ����"
//    };

//}
