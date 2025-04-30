using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


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
    [SerializeField]
    private LifeStatData lifeStatData;

    [SerializeField]
    private Slider experienceSlider;

    [SerializedDictionary, SerializeField]
    private SerializedDictionary<LifeSkillType, int> skillLevelTable = new SerializedDictionary<LifeSkillType, int>();
    public SerializedDictionary<LifeSkillType, int> SkillLevelTable => skillLevelTable;

    public UnityEvent onLevelUpEvent;
    public UnityEvent<int> OnChangeSkillPointEvent;
    public UnityEvent<LifeSkillType> OnChangeSkillLevelEvent;
    public UnityEvent<LifeSkillType, int, int> OnChangeSkillLevelCountEvent;

    private List<float> currentSkillStatValueList = new List<float>();
    public List<float> SkillStatValueList => currentSkillStatValueList;

    private int skillPoint = 0;

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
        levelUpExperience = lifeStatData.LevelList[currentLevel - 1];

        currentSkillStatValueList.Clear();
        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            currentSkillStatValueList.Add(0f);
        }

        if (SaveLoadManager.Data == null)
        {
            return;
        }
        Load();
        OnChangeExperienceSlider();
    }

    private void Start()
    {
        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            if (currentSkillStatValueList[i] != 0f)
            {
                OnChangeSkillLevelEvent?.Invoke((LifeSkillType)i);
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

        OnChangeExperienceSlider();
    }

    private void OnChangeExperienceSlider()
    {
        if (experienceSlider != null)
        {
            experienceSlider.value = currentExperience / levelUpExperience;
        }

    }

    public void OnSkillLevelUp(int skilType)
    {
        var type = (LifeSkillType)skilType;

        if (skillLevelTable.ContainsKey(type))
        {
            ++skillLevelTable[type];
            currentSkillStatValueList[skilType] = lifeStatData.LifeSkillStatTable[type] * skillLevelTable[type];
        }
        else
        {
            skillLevelTable.Add(type, 1);
            currentSkillStatValueList[skilType] = lifeStatData.LifeSkillStatTable[type];
        }

        --skillPoint;
        OnChangeSkillPointEvent?.Invoke(skillPoint);
        OnChangeSkillLevelEvent?.Invoke(type);
    }

    protected override void LevelUp()
    {
        if (currentLevel == maxLevel)
        {
            return;
        }

        ++currentLevel;
        ++skillPoint;

        levelUpExperience = lifeStatData.LevelList[currentLevel - 1];

        OnChangeSkillPointEvent?.Invoke(skillPoint);
        onLevelUpEvent?.Invoke();
        // SkilUp(Random.Range(0, (int)LifeSkillType.End - 1));
    }

    public void OnResetSkill()
    {
        skillPoint = currentLevel;

        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            currentSkillStatValueList[i] = 0f;
            OnChangeSkillLevelEvent?.Invoke((LifeSkillType)i);
        }

        OnChangeSkillPointEvent?.Invoke(skillPoint);
    }

    public void OnSkillLevelUp(LifeSkillType lifeSkillType)
    {
        if(skillPoint == 0)
        {
            return;
        }

        if (skillLevelTable.ContainsKey(lifeSkillType))
        {
            ++skillLevelTable[lifeSkillType];
            currentSkillStatValueList[(int)lifeSkillType] = lifeStatData.LifeSkillStatTable[lifeSkillType] * skillLevelTable[lifeSkillType];
        }
        else
        {
            skillLevelTable.Add(lifeSkillType, 1);
            currentSkillStatValueList[(int)lifeSkillType] = lifeStatData.LifeSkillStatTable[lifeSkillType];
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
        levelUpExperience = lifeStatData.LevelList[currentLevel];

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

            skillLevelTable.Add((LifeSkillType)i, list[i]);
            currentSkillStatValueList[i] = lifeStatData.LifeSkillStatTable[(LifeSkillType)i] * list[i];
        }
    }

    public void Save()
    {
        var levelInfo = new LevelStatInfo();

        levelInfo.level = currentLevel;
        levelInfo.skillPoint = skillPoint;
        levelInfo.Experience = currentExperience;
        levelInfo.skillLevelList = new List<int>();

        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            if (skillLevelTable.TryGetValue((LifeSkillType)i, out var value))
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
