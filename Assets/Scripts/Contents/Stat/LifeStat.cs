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

    private List<float> currentSkillStatValue = new List<float>();

    public float Damage { get {return currentSkillStatValue[0];} }
    public float MoveSpeed { get {return currentSkillStatValue[1];} }
    public float AttackSpeed { get {return currentSkillStatValue[2];} }
    public float Hungur { get {return currentSkillStatValue[3];} }
    public float Thirst { get { return currentSkillStatValue[4]; } }

    private void Awake()
    {
        levelUpExperience = lifeStatData.LevelList[currentLevel - 1];

        currentSkillStatValue.Clear();
        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            currentSkillStatValue.Add(1f);
        }

        OnChangeExperienceSlider();
    }

    public void OnAddExperience(float experience)
    {
        currentExperience += experience;

        if(currentExperience >= levelUpExperience)
        {
            while (currentExperience >= levelUpExperience)
            {
                currentExperience -= levelUpExperience;
                LevelUp();
            }
        }

        OnChangeExperienceSlider();
    }
    public void Load()
    {
    }

    public void Save()
    {
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

        if(skillLevelTable.ContainsKey(type))
        {
            ++skillLevelTable[type];
            currentSkillStatValue[skilType] = lifeStatData.LifeSkillStatTable[type] * skillLevelTable[type];
        }
        else
        {
            skillLevelTable.Add(type, 1);
            currentSkillStatValue[skilType] = lifeStatData.LifeSkillStatTable[type];
        }
    }
    
    protected override void LevelUp()
    {
        if(currentLevel == maxLevel)
        {
            return;
        }

        ++currentLevel;
        levelUpExperience = lifeStatData.LevelList[currentLevel - 1];
        onLevelUpEvent?.Invoke();
        // SkilUp(Random.Range(0, (int)LifeSkillType.End - 1));
    }
}
