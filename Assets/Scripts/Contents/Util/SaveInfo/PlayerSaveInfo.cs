using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct PlayerSaveInfo
{
    public float hp;
    public float[] survivalStatValues;
}

[System.Serializable]

public struct LevelStatInfo
{
    public int level;
    public int skillPoint;
    public float Experience;
    public List<int> skillLevelList;
}