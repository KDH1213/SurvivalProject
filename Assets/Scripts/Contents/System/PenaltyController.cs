using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PenaltyController : MonoBehaviour, IAct
{
    //[field: SerializeField]
    //public SurvivalStatData SurvivalStatData { get; private set; }
    private SerializedDictionary<SurvivalStatType, IPenalty> penaltyTable = new SerializedDictionary<SurvivalStatType, IPenalty>();
    [SerializeField]
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        penaltyTable.Clear();

        var penaltys = GetComponents<IPenalty>();

        foreach (var penalty in penaltys)
        {
            penaltyTable.Add(penalty.PenaltyType, penalty);
        }

    }

    public float CalculatePenaltySpeed()
    {
        if(penaltyTable[SurvivalStatType.Hunger].IsOnPenalty)
        {
            return 0.2f;
        }
        else
        {
            return 1f;
        }
    }

    public float CalculatePenaltyAttackSpeed()
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

    public void StartPenaltyHpDown()
    {
        var takeDamage = GetComponent<AttackedTakeDamage>();
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = playerStats.GetStat(StatType.HP).MaxValue * 0.01f;
        takeDamage.OnAttack(this.gameObject, damageInfo);
    }
}
