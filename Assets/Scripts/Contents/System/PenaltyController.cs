using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PenaltyController : MonoBehaviour, IAct, ISaveLoadData
{
    //[field: SerializeField]
    //public SurvivalStatData SurvivalStatData { get; private set; }
    private SerializedDictionary<SurvivalStatType, SurvivalStatBehaviour> penaltyTable = new SerializedDictionary<SurvivalStatType, SurvivalStatBehaviour>();
    public SerializedDictionary<SurvivalStatType, SurvivalStatBehaviour> PenaltyTable {  get { return penaltyTable; } }
    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private ActInfoData actInfoData;    

    // TODO :: 패널티 수치 데이터 테이블 나오면 수정
    [SerializeField]
    private float speedDownPersent = 0.2f;
    [SerializeField]
    private float penaltyAttackSpeedPersent = 0.5f;

    private float currentSpeedPersent = 1f;
    private float currentAttackSpeedPersent = 1f;


    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        penaltyTable.Clear();

        var penaltys = GetComponents<SurvivalStatBehaviour>();

        foreach (var penalty in penaltys)
        {
            penaltyTable.Add(penalty.PenaltyType, penalty);
            penalty.OnStartPenaltyEvent.AddListener(OnStartPanelty);
            penalty.OnEndPenaltyEvent.AddListener(OnEndPanelty);
        }

    }

    public float CalculatePenaltySpeed()
    {
        return currentSpeedPersent;
    }

    public float CalculatePenaltyAttackSpeed()
    {
        return currentAttackSpeedPersent;
    }

    public void PlayAct(int id)
    {
        var actInfoList = actInfoData.actDataTable[(ActType)id].actInfoList; // ActManager.actDataTable[id].actInfoList;
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

    public void OnStartPanelty(SurvivalStatType survivalStatType)
    {
        switch (survivalStatType)
        {
            case SurvivalStatType.Fatigue:
                currentAttackSpeedPersent = penaltyAttackSpeedPersent;
                break;
            case SurvivalStatType.Hunger:
                currentSpeedPersent = speedDownPersent;
                break;
            case SurvivalStatType.Thirst:
                break;
            case SurvivalStatType.Temperature:
                break;
            case SurvivalStatType.Hygiene:
                break;
            case SurvivalStatType.End:
            default:
                break;
        }
    }

    public void OnEndPanelty(SurvivalStatType survivalStatType)
    {
        switch (survivalStatType)
        {
            case SurvivalStatType.Fatigue:
                currentAttackSpeedPersent = 1f;
                break;
            case SurvivalStatType.Hunger:
                currentSpeedPersent = 1f;
                break;
            case SurvivalStatType.Thirst:
                break;
            case SurvivalStatType.Temperature:
                break;
            case SurvivalStatType.Hygiene:
                break;
            case SurvivalStatType.End:
            default:
                break;
        }
    }

    public void Save()
    {
        var SurvivalStatBehaviours = GetComponents<SurvivalStatBehaviour>();

        foreach (var SurvivalStatBehaviour in SurvivalStatBehaviours)
        {
            SurvivalStatBehaviour.Save();
        }
    }

    public void Load()
    {
    }
}
