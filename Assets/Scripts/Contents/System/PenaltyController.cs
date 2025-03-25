using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PenaltyController : MonoBehaviour, IAct
{
    //[field: SerializeField]
    //public SurvivalStatData SurvivalStatData { get; private set; }
    private SerializedDictionary<SurvivalStatType, IPenalty> penaltyTable = new SerializedDictionary<SurvivalStatType, IPenalty>();



    private void Awake()
    {
        penaltyTable.Clear();

        var debuffs = GetComponents<IPenalty>();

        foreach (var debuff in debuffs)
        {
            penaltyTable.Add(debuff.PenaltyType, debuff);
        }

    }

    public float CalculateSpeedPenalty()
    {
        // if (survivalStatTable[SurvivalStatType.Hunger] < )
        return 1f;
    }

    public float CalculateAttackSpeedPenalty()
    {
        return 1f;
    }

    public void PlayAct(int id)
    {
        var actInfoList = ActManager.actDataTable[id].actInfoList;
        foreach (var act in actInfoList)
        {
            penaltyTable[act.penaltyType].AddPenaltyValue(act.value);
        }
    }
}
