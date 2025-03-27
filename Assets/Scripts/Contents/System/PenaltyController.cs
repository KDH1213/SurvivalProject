using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PenaltyController : MonoBehaviour, IAct
{
    //[field: SerializeField]
    //public SurvivalStatData SurvivalStatData { get; private set; }
    private SerializedDictionary<SurvivalStatType, IPenalty> penaltyTable = new SerializedDictionary<SurvivalStatType, IPenalty>();
    [SerializeField]
    private PlayerStats playerStats;

    // TODO :: 패널티 수치 데이터 테이블 나오면 수정
    [SerializeField]
    private float speedDownPersent = 0.2f;
    [SerializeField]
    private float penaltyAttackSpeedPersent = 0.5f;


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
        if (penaltyTable[SurvivalStatType.Hunger].IsOnPenalty)
        {
            return speedDownPersent;
        }
        else
        {
            return 1f;
        }
    }

    public float CalculatePenaltyAttackSpeed()
    {
        if (penaltyTable[SurvivalStatType.Fatigue].IsOnPenalty)
        {
            return penaltyAttackSpeedPersent;
        }
        else
        {
            return 1f;
        }
    }

    public void PlayAct(int id)
    {
        var actInfoList = ActManager.actDataTable[id].actInfoList;
        foreach (var act in actInfoList)
        {
            penaltyTable[act.penaltyType].AddPenaltyValue(act.value);
        }
    }

    // TODO ::Hunger컴포넌트에 연결
    public void OnStartPenaltyHpDown()
    {
        var takeDamage = GetComponent<AttackedTakeDamage>();
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = playerStats.GetStat(StatType.HP).MaxValue * 0.01f;
        takeDamage.OnAttack(this.gameObject, damageInfo);
    }

}
