using System;

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

public static class QuestTableIds
{
    public static readonly string[] String =
    {
        "QuestTable",
    };
}
public static class DropTableIds
{
    public static readonly string[] String =
    {
        "DropTable",
    };
}

public static class PlayerLevelTableIds
{
    public static readonly string[] String =
    {
        "PlayerLevelTable",
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
    public static readonly string QuestSystem = "QuestSystem";
    public static readonly string PlacementSystem = "PlacementSystem";
    public static readonly string MiniMap = "MiniMap";
    public static readonly string MonsterSpawnSystem = "MonsterSpawnSystem";
    public static readonly string UIDebuffIcon = "UIDebuffIcon";
    public static readonly string AdManager = "AdManager";
}

public static class SceneName
{
    public static readonly string Develop = "Assets/Scenes/Develop/Develop.unity";
    public static readonly string Stage1 = "Stage1";
    public static readonly string Stage2 = "Stage2";
    public static readonly string Stage3 = "Stage3";
    public static readonly string LoadScene = "LoadingScene";

    public static int GetStageNumber(string sceneName)
    {
        if(sceneName == Develop)
        {
            return SceneStage.Develop;
        }
        else if (sceneName == Stage1)
        {
            return SceneStage.Stage1;
        } 
        else if (sceneName == Stage2)
        {
            return SceneStage.Stage2;
        }
        else if (sceneName == Stage3)
        {
            return SceneStage.Stage3;
        }
        else if (sceneName == LoadScene)
        {
            return SceneStage.LoadScene;
        }
        else
        {
            throw new ArgumentOutOfRangeException("존재하지 않는 Scene");
        }
    }
}

public static class SceneStage
{
    public static readonly int Develop = -1;
    public static readonly int Stage1 = 1;
    public static readonly int Stage2 = 2;
    public static readonly int Stage3 = 3;
    public static readonly int LoadScene = 999;
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

    public static readonly string[] LifeSkillTypeNameFormat =
    {
        "공격력 증가 : {0}",
        "이동속도 증가 : {0}",
        "공격속도 증가 : {0}",
        "배고픔 주기 감소 : {0}",
        "갈증 주기 감소 : {0}",
        "방어력 증가 : {0}",
        "체력 증가 : {0}",
        "피로 덜 느낌 : {0}",
    };
}