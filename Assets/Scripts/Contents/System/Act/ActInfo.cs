using System.Collections.Generic;

[System.Serializable]
public struct ActInfo
{
    public SurvivalStatType penaltyType;
    public float value;
}

[System.Serializable]
public class ActData
{
    public int id;
    public List<ActInfo> actInfoList = new List<ActInfo>();
}