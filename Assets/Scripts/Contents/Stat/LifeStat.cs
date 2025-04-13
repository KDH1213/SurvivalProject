using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LifeStat : LevelStat, ISaveLoadData
{
    [SerializeField]
    private LifeStatData lifeStatData;

    [SerializeField]
    private Slider experienceSlider;

    [SerializedDictionary, SerializeField]
    private SerializedDictionary<LifeSkillType, int> skillLevelTable = new SerializedDictionary<LifeSkillType, int>();

    public UnityEvent onLevelUpEvent;
    public UnityEvent<int> OnChangeSkillPointEvent;
    public UnityEvent<LifeSkillType> OnChangeSkillLevelEvent;

    private List<float> currentSkillStatValue = new List<float>();

    private int skillPoint = 0;

    public float Damage { get { return currentSkillStatValue[0]; } }
    public float MoveSpeed { get { return currentSkillStatValue[1]; } }
    public float AttackSpeed { get { return currentSkillStatValue[2]; } }
    public float Hungur { get { return currentSkillStatValue[3]; } }
    public float Thirst { get { return currentSkillStatValue[4]; } }

    private void Awake()
    {
        levelUpExperience = lifeStatData.LevelList[currentLevel - 1];

        currentSkillStatValue.Clear();
        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            currentSkillStatValue.Add(0f);
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
            if (currentSkillStatValue[i] != 0f)
            {
                OnChangeSkillLevelEvent?.Invoke((LifeSkillType)i);
            }
        }
    }

    public void OnAddExperience(float experience)
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
            currentSkillStatValue[skilType] = lifeStatData.LifeSkillStatTable[type] * skillLevelTable[type];
        }
        else
        {
            skillLevelTable.Add(type, 1);
            currentSkillStatValue[skilType] = lifeStatData.LifeSkillStatTable[type];
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
    public void Load()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        var levelStatInfo = SaveLoadManager.Data.PlayerSaveData.levelStatInfo;
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
            currentSkillStatValue[i] = lifeStatData.LifeSkillStatTable[(LifeSkillType)i];
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

        SaveLoadManager.Data.PlayerSaveData.levelStatInfo = levelInfo;
    }
}
