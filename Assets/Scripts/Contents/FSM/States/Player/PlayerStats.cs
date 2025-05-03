using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : CharactorStats, ISaveLoadData
{
    [SerializeField]
    private PenaltyController survivalStats;

    [SerializeField]
    private LifeStat lifeStat;

    [SerializeField]
    private Slider HpBarSlider;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private float invincibilityTime = 3f;

    [SerializeField]
    private float takeDamageinvincibilityTime = 0.1f; 

    private Coroutine resurrectionCoroutine;
    private Coroutine takeDamageInvincibilityCoroutine;

    private WaitForSeconds takeDamageInvincibilityWaitForSeconds;

    private float currentSpeed = 1f;
    private float currentAttackSpeed = 1f;
    private float currentDamage = 1f;
    private float currentDefence = 0f;
    private readonly string hpFormat = "{0} / {1}";
    private const string pointFormat = "F0";

    public UnityAction<float> onChangeAttackSpeedValue;
    public UnityAction<float> onChangeSpeedValue;
    public UnityAction<float> onChangDamageValue;
    public UnityAction<float> onChangeDefenceValue;
    public UnityAction<float> onChangeColdResistanceValue;
    public UnityAction<float> onChangeHeatResistanceValue;
    public UnityEvent<ItemData> onEquipmentItemEvent;
    public UnityEvent<ItemData> onUnEquipmentItemEvent;


    protected override void Awake()
    {
        originalData.CopyStat(ref currentStatTable);

        if (survivalStats == null)
        {
            survivalStats = GetComponent<PenaltyController>();
        }

        var playerAssetController = GetComponent<PlayerAssetController>();
        onEquipmentItemEvent.AddListener(playerAssetController.OnEquipmentItem);
        onUnEquipmentItemEvent.AddListener(playerAssetController.OnUnEquipmentItem);

        damegedEvent.AddListener(OnTakeDamage);
        takeDamageInvincibilityWaitForSeconds = new WaitForSeconds(takeDamageinvincibilityTime);

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        var playerFSM = GetComponent<PlayerFSM>();
        playerFSM.onActAction += survivalStats.OnPlayAct;

        deathEvent.AddListener(() => playerFSM.ChangeState(PlayerStateType.Death));

        currentStatTable[StatType.MovementSpeed].OnChangeValueAction(OnChangeSpeedValue);
        currentStatTable[StatType.AttackSpeed].OnChangeValueAction(OnChangeAttackSpeedValue);
        currentStatTable[StatType.BasicAttackPower].OnChangeValueAction(OnChangeDamageValue);
        currentStatTable[StatType.Defense].OnChangeValueAction(OnChangeDefenceValue);

        currentSpeed = currentStatTable[StatType.MovementSpeed].Value;
        currentAttackSpeed = currentStatTable[StatType.AttackSpeed].Value;
        currentDamage = currentStatTable[StatType.BasicAttackPower].Value;
        currentDefence = currentStatTable[StatType.Defense].Value;

        OnChangeHp();

        lifeStat.OnChangeSkillLevelEvent.AddListener(OnChangeLifeStat);
    }

    public void Start()
    {
        onChangeAttackSpeedValue?.Invoke(currentAttackSpeed);
        onChangeSpeedValue?.Invoke(currentSpeed);
        onChangDamageValue?.Invoke(currentDamage);
        onChangeDefenceValue?.Invoke(currentDefence);
    }

    public float Speed
    {
        get
        {
            return currentSpeed * survivalStats.CalculatePenaltySpeed();
        }
    }
    public float Hp
    {
        get
        {
            return currentStatTable[StatType.HP].Value;
        }
    }

    public float AttackSpeed
    {
        get
        {
            return currentAttackSpeed * survivalStats.CalculatePenaltyAttackSpeed();
        }
    }

    public override float AttackPower
    {
        get
        {
            return currentDamage;
        }
    }

    public float Defense
    {
        get
        {
            return currentStatTable[StatType.Defense].Value + lifeStat.Defence;
        }
    }

    public void AddStatType(StatType type, float addValue)
    {
        if (currentStatTable.TryGetValue(type, out var statValue))
        {
            statValue.AddValue(addValue);
        }
    }
    public void OnChangeHp() //*HP ����
    {
        var value = currentStatTable[StatType.HP];
        HpBarSlider.value = value.Value / value.MaxValue;
        hpText.text = string.Format(hpFormat, value.Value.ToString(pointFormat), value.MaxValue.ToString(pointFormat));
    }

    public void OnTakeDamage()
    {
        if(!IsDead && takeDamageInvincibilityCoroutine == null)
        {
            takeDamageInvincibilityCoroutine = StartCoroutine(CoTakeDamageInvincibility());
        }
    }

    public void OnEquipmentItem(ItemData itemData)
    {
        var statInfoList = itemData.GetItemInfoList();
        onEquipmentItemEvent?.Invoke(itemData);

        foreach (var statInfo in statInfoList)
        {
            if (statInfo.statType == StatType.AttackSpeed)
            {
                currentStatTable[statInfo.statType].SetValue(statInfo.value);
            }
            else
            {
                currentStatTable[statInfo.statType].AddValue(statInfo.value);
            }
        }
    }

    public void OnUnEquipmentItem(ItemData itemData)
    {
        var statInfoList = itemData.GetItemInfoList();
        onUnEquipmentItemEvent?.Invoke(itemData);

        foreach (var statInfo in statInfoList)
        {
            if (statInfo.statType == StatType.AttackSpeed)
            {
                currentStatTable[statInfo.statType].SetValue(originalData.StatTable[statInfo.statType].Value);
            }
            else
            {
                currentStatTable[statInfo.statType].AddValue(-statInfo.value);
            }
        }
    }

    public void OnUseItem(ItemData itemData)
    {
        var itemUseEffectInfoList = itemData.ItemUseEffectInfoList;

        int count = itemUseEffectInfoList.Count;

        for (int i = 0; i < count; ++i)
        {
            switch (itemUseEffectInfoList[i].itemUseEffectType)
            {
                case ItemUseEffectType.None:
                    break;
                case ItemUseEffectType.Hp:
                    currentStatTable[StatType.HP].AddValue(itemUseEffectInfoList[i].value);
                    OnChangeHp();
                    break;
                case ItemUseEffectType.Fatigue:
                    survivalStats.PenaltyTable[SurvivalStatType.Fatigue].SubPenaltyValue(itemUseEffectInfoList[i].value);
                    break;
                case ItemUseEffectType.Hunger:
                    survivalStats.PenaltyTable[SurvivalStatType.Hunger].AddPenaltyValue(itemUseEffectInfoList[i].value);
                    break;
                case ItemUseEffectType.Thirst:
                    survivalStats.PenaltyTable[SurvivalStatType.Thirst].AddPenaltyValue(itemUseEffectInfoList[i].value);
                    break;
                default:
                    break;
            }
        }
    }

    public void OnChangeLifeStat(LifeSkillType type)
    {
        switch (type)
        {
            case LifeSkillType.Damage:
                currentDamage = currentStatTable[StatType.BasicAttackPower].Value + lifeStat.Damage;
                onChangDamageValue?.Invoke(currentDamage);
                break;
            case LifeSkillType.MoveSpeed:
                currentSpeed = currentStatTable[StatType.MovementSpeed].Value + lifeStat.MoveSpeed;
                onChangeSpeedValue?.Invoke(currentSpeed);
                break;
            case LifeSkillType.AttackSpeed:
                currentAttackSpeed = currentStatTable[StatType.AttackSpeed].Value + lifeStat.AttackSpeed;
                onChangeAttackSpeedValue?.Invoke(currentAttackSpeed);
                break;
            case LifeSkillType.Hungur:
                GetComponent<HungerStat>().OnSetHungerSkillValue(lifeStat.Hungur);
                break;
            case LifeSkillType.Thirst:
                GetComponent<ThirstStat>().OnSetThirstSkillValue(lifeStat.Thirst);
                break;
            case LifeSkillType.Defence:
                currentDefence = currentStatTable[StatType.Defense].Value + lifeStat.Defence;
                onChangeDefenceValue?.Invoke(currentDefence);
                break;
            case LifeSkillType.HP:
                currentStatTable[StatType.HP].AddMaxValue(5f);
                OnChangeHp();
                break;
            case LifeSkillType.Fatigue:
                GetComponent<FatigueStat>().OnSetFatigueSkillValue(lifeStat.Fatigue);
                break;
            case LifeSkillType.End:
                break;
            default:
                break;
        }
    }

    public void OnChangeSpeedValue(float value)
    {
        currentSpeed = value + lifeStat.MoveSpeed;
        onChangeSpeedValue?.Invoke(currentSpeed);
    }

    public void OnChangeAttackSpeedValue(float value)
    {
        currentAttackSpeed = value + lifeStat.AttackSpeed; 
        onChangeAttackSpeedValue?.Invoke(currentAttackSpeed);
    }

    public void OnChangeDamageValue(float value)
    {
        currentDamage = value + lifeStat.Damage;
        onChangDamageValue?.Invoke(currentDamage);
    }

    public void OnChangeDefenceValue(float value)
    {
        currentDefence = value + lifeStat.Defence;
        onChangeDefenceValue?.Invoke(currentDefence);
    }

    public void OnResurrection()
    {
        IsDead = false;
        CanHit = false;

        resurrectionCoroutine = StartCoroutine(CoResurrection());
        var hpStat = currentStatTable[StatType.HP];
        hpStat.SetValue(hpStat.MaxValue);
        OnChangeHp();
    }

    private IEnumerator CoResurrection()
    {
        CanHit = false;
        yield return new WaitForSeconds(invincibilityTime);
        CanHit = true;
    }

    private IEnumerator CoTakeDamageInvincibility()
    {
        CanHit = false;
        yield return takeDamageInvincibilityWaitForSeconds;
        CanHit = true;
        takeDamageInvincibilityCoroutine = null;
    }

    public void Save()
    {
        lifeStat.Save();
        survivalStats.Save();
    }

    public void Load()
    {
        // lifeStat.Load();

        if(SaveLoadManager.Data.levelStatInfo.skillLevelList.Count >(int)LifeSkillType.HP)
        {
            currentStatTable[StatType.HP].AddMaxValue(SaveLoadManager.Data.levelStatInfo.skillLevelList[(int)LifeSkillType.HP] * 5f);
        }

        currentStatTable[StatType.HP].SetValue(SaveLoadManager.Data.playerSaveInfo.hp);

        var equipmentItemIDList = SaveLoadManager.Data.equipmentItemIDList;

        for (int i = 0; i < equipmentItemIDList.Count; ++i)
        {
            if (equipmentItemIDList[i] == -1)
            {
                continue;
            }

            // 해당 부분 SaveLoad에 따라 ItemData 값 다르게 세팅
            var itemData = DataTableManager.ItemTable.Get(equipmentItemIDList[i]);

            if (itemData == null)
            {
                continue;
            }

            OnEquipmentItem(itemData);
        }

    }
}
