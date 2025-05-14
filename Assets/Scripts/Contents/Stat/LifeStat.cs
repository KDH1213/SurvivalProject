using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillInfo
{
    public int Value { get; private set; }
    public int MaxValue { get; private set; }

    public System.Action<int, int> onChangeValueAction;

    public void AddValue()
    {
        ++Value;
        onChangeValueAction?.Invoke(Value, MaxValue);
    }
}

public class LifeStat : LevelStat, ISaveLoadData
{
    [field: SerializeField]
    public LifeStatData SkillStatData { get; private set; }

    [SerializedDictionary, SerializeField]
    private SerializedDictionary<SkillType, int> skillLevelTable = new SerializedDictionary<SkillType, int>();
    public SerializedDictionary<SkillType, int> SkillLevelTable => skillLevelTable;

    public UnityEvent onLevelUpEvent;
    public UnityEvent<int> onChangeLevelEvent;

    public UnityEvent<int> OnChangeSkillPointEvent;
    public UnityEvent<SkillType> OnChangeSkillLevelEvent;
    public UnityEvent<SkillType, int, int> OnChangeSkillLevelCountEvent;

    public UnityEvent<float, float> onChangeExperienceEvent;

    private List<float> currentSkillStatValueList = new List<float>();
    public List<float> SkillStatValueList => currentSkillStatValueList;

    private int skillPoint = 0;
    public int SkillPoint => skillPoint;

    public float Damage { get { return currentSkillStatValueList[0]; } }
    public float MoveSpeed { get { return currentSkillStatValueList[1]; } }
    public float AttackSpeed { get { return currentSkillStatValueList[2]; } }
    public float Hungur { get { return currentSkillStatValueList[3]; } }
    public float Thirst { get { return currentSkillStatValueList[4]; } }
    public float Defence { get { return currentSkillStatValueList[5]; } }
    public float HP { get { return currentSkillStatValueList[6]; } }
    public float Fatigue { get { return currentSkillStatValueList[7]; } }

    private void Awake()
    {
        levelUpExperience = DataTableManager.PlayerLevelTable.Get(0); // lifeStatData.LevelList[currentLevel - 1];
        maxLevel = DataTableManager.PlayerLevelTable.PlayerLevelDataList.Count;

        currentSkillStatValueList.Clear();
        for (int i = 0; i < (int)SkillType.End; ++i)
        {
            currentSkillStatValueList.Add(0f);
        }

        if (SaveLoadManager.Data == null)
        {
            return;
        }
        Load();
    }

    private void Start()
    {
        onChangeExperienceEvent?.Invoke(currentExperience, levelUpExperience);
        onChangeLevelEvent?.Invoke(currentLevel);

        for (int i = 0; i < (int)SkillType.End; ++i)
        {
            if (currentSkillStatValueList[i] != 0f)
            {
                OnChangeSkillLevelEvent?.Invoke((SkillType)i);
            }
        }
    }

    public void OnAddExperience(GameObject target, float experience)
    {
        currentExperience += experience;

        if (currentExperience >= levelUpExperience)
        {
            while (currentExperience >= levelUpExperience)
            {
                currentExperience -= levelUpExperience;
                LevelUp();
            }
        }

        onChangeExperienceEvent?.Invoke(currentExperience, levelUpExperience);

    }

    public void OnSkillLevelUp(int skilType)
    {
        var type = (SkillType)skilType;

        if (skillLevelTable.ContainsKey(type))
        {
            ++skillLevelTable[type];
            currentSkillStatValueList[skilType] = SkillStatData.SkillStatTable[type][skillLevelTable[type]];
        }
        else
        {
            skillLevelTable.Add(type, 1);
            currentSkillStatValueList[skilType] = SkillStatData.SkillStatTable[type][0];
        }

        --skillPoint;
        OnChangeSkillPointEvent?.Invoke(skillPoint);
        OnChangeSkillLevelEvent?.Invoke(type);
        OnChangeSkillLevelCountEvent?.Invoke(type, skillLevelTable[type], 100);
    }

    protected override void LevelUp()
    {
        if (currentLevel == maxLevel)
        {
            return;
        }

        ++currentLevel;
        ++skillPoint;

        levelUpExperience = DataTableManager.PlayerLevelTable.Get(currentLevel - 1);// lifeStatData.LevelList[currentLevel - 1];

        SoundManager.Instance.OnSFXPlay(transform, (int)SoundType.PlayerLevelUp);

        OnChangeSkillPointEvent?.Invoke(skillPoint);
        onLevelUpEvent?.Invoke();
        onChangeLevelEvent?.Invoke(currentLevel);

        // SkilUp(Random.Range(0, (int)LifeSkillType.End - 1));
    }

    public void OnResetSkill()
    {
        skillPoint = currentLevel;

        for (int i = 0; i < (int)SkillType.End; ++i)
        {
            var lifeType = (SkillType)i;
            currentSkillStatValueList[i] = 0f;
            OnChangeSkillLevelEvent?.Invoke(lifeType);
            skillLevelTable[lifeType] = 0;
            OnChangeSkillLevelCountEvent?.Invoke(lifeType, 0, 100);
        }

        OnChangeSkillPointEvent?.Invoke(skillPoint);
    }

    public void OnSkillLevelUp(SkillType lifeSkillType)
    {
        if(skillPoint == 0)
        {
            return;
        }

        if (skillLevelTable.ContainsKey(lifeSkillType))
        {
            ++skillLevelTable[lifeSkillType];
            currentSkillStatValueList[(int)lifeSkillType] = SkillStatData.SkillStatTable[lifeSkillType][skillLevelTable[lifeSkillType]];
        }
        else
        {
            skillLevelTable.Add(lifeSkillType, 1);
            currentSkillStatValueList[(int)lifeSkillType] = SkillStatData.SkillStatTable[lifeSkillType][0];
        }

        --skillPoint;
        OnChangeSkillPointEvent?.Invoke(skillPoint);
        OnChangeSkillLevelEvent?.Invoke(lifeSkillType);
        OnChangeSkillLevelCountEvent?.Invoke(lifeSkillType, skillLevelTable[lifeSkillType], 100);
    }

    public void Load()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        var levelStatInfo = SaveLoadManager.Data.levelStatInfo;
        currentLevel = levelStatInfo.level;
        skillPoint = levelStatInfo.skillPoint;
        currentExperience = levelStatInfo.Experience;
        levelUpExperience = DataTableManager.PlayerLevelTable.Get(currentLevel); //lifeStatData.LevelList[currentLevel];

        if(skillPoint != 0)
        {
            OnChangeSkillPointEvent?.Invoke(skillPoint);
        }

        var list = levelStatInfo.skillLevelList;

        if (levelStatInfo.skillLevelList == null)
        {
            return;
        }

        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] == 0)
            {
                continue;
            }

            skillLevelTable.Add((SkillType)i, list[i]);
            currentSkillStatValueList[i] = SkillStatData.SkillStatTable[(SkillType)i][list[i]];
        }
    }

    public void Save()
    {
        var levelInfo = new LevelStatInfo();

        levelInfo.level = currentLevel;
        levelInfo.skillPoint = skillPoint;
        levelInfo.Experience = currentExperience;
        levelInfo.skillLevelList = new List<int>();

        for (int i = 0; i < (int)SkillType.End; ++i)
        {
            if (skillLevelTable.TryGetValue((SkillType)i, out var value))
            {
                levelInfo.skillLevelList.Add(value);
            }
            else
            {
                levelInfo.skillLevelList.Add(0);
            }
        }

        SaveLoadManager.Data.levelStatInfo = levelInfo;
    }
}
