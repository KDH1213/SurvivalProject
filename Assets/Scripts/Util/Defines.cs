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
