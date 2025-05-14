using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UISkillView : MonoBehaviour
{
    [SerializeField]
    private Transform noramlContentPoint;

    [SerializeField]
    private Transform lifeContentPoint;

    [SerializeField]
    private Transform craftingContentPoint;

    [SerializeField]
    private UISkillSlot uISkillSlotPrefab;

    [SerializeField]
    private TextMeshProUGUI skillNameText;

    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;

    [SerializeField]
    private TextMeshProUGUI skillPointText;

    [SerializeField]
    private List<TextMeshProUGUI> normalTextList = new List<TextMeshProUGUI>();

    private List<float> valueList;

    public UnityEvent<SkillType> onSkillLevelUpEvent;
    public UnityEvent onResetSkillEvent;

    private SkillType currentLifeSkillType = SkillType.End;

    private string normalText;
    private string lifeText;
    private string craftingText;
    private readonly string skillPointFormat = "스킬 포인트 : {0}";

    private void OnDisable()
    {
        currentLifeSkillType = SkillType.End;
    }

    private void Awake()
    {
        var lifeStat = GameObject.FindWithTag(Tags.Player).GetComponent<LifeStat>();

        var skillStatData = lifeStat.SkillStatData;
        onSkillLevelUpEvent.AddListener(lifeStat.OnSkillLevelUp);
        lifeStat.OnChangeSkillLevelCountEvent.AddListener(OnChangeSkillLevel);
        lifeStat.OnChangeSkillPointEvent.AddListener(OnChangeSkillPoint);
        onResetSkillEvent.AddListener(lifeStat.OnResetSkill);
        OnChangeSkillPoint(lifeStat.SkillPoint);

        valueList = lifeStat.SkillStatValueList;

        var normalSkillTypeList = skillStatData.NormalSkillTypeList;
        int count = normalSkillTypeList.Count;

        for (int i = 0; i < count; ++i)
        {
            var createSkillSlot = Instantiate(uISkillSlotPrefab, noramlContentPoint);
            createSkillSlot.InitializedInfo(skillStatData.SkillSpriteTable[normalSkillTypeList[i]], normalSkillTypeList[i], (int)normalSkillTypeList[i], (int)normalSkillTypeList[i]);
            createSkillSlot.onLifeSkillUpEvent.AddListener(OnSkillLevelUp);
            lifeStat.OnChangeSkillLevelCountEvent.AddListener((lifeType, currentLevel, maxLevel) =>
            {
                if(createSkillSlot.LifeSkillType == lifeType)
                {
                    createSkillSlot.OnChangeSkillLevel(currentLevel, skillStatData.SkillStatTable[normalSkillTypeList[i]].Count);
                }
            });

            createSkillSlot.onClickAction += OnSetDescription;

            if (lifeStat.SkillLevelTable.TryGetValue((SkillType)i, out var value))
            {
                OnChangeSkillLevel((SkillType)i, value, skillStatData.SkillStatTable[normalSkillTypeList[i]].Count);
            }
        }

        var lifeSkillTypeList = skillStatData.LifeSkillTypeList;
        count = lifeSkillTypeList.Count;
        for (int i = 0; i < count; ++i)
        {
            var createSkillSlot = Instantiate(uISkillSlotPrefab, lifeContentPoint);
            createSkillSlot.InitializedInfo(skillStatData.SkillSpriteTable[lifeSkillTypeList[i]], lifeSkillTypeList[i], (int)lifeSkillTypeList[i], (int)lifeSkillTypeList[i]);
            createSkillSlot.onLifeSkillUpEvent.AddListener(OnSkillLevelUp);
            lifeStat.OnChangeSkillLevelCountEvent.AddListener((lifeType, currentLevel, maxLevel) =>
            {
                if (createSkillSlot.LifeSkillType == lifeType)
                {
                    createSkillSlot.OnChangeSkillLevel(currentLevel, skillStatData.SkillStatTable[lifeSkillTypeList[i]].Count);
                }
            });

            createSkillSlot.onClickAction += OnSetDescription;

            if (lifeStat.SkillLevelTable.TryGetValue((SkillType)i, out var value))
            {
                OnChangeSkillLevel((SkillType)i, value, skillStatData.SkillStatTable[lifeSkillTypeList[i]].Count);
            }
        }

        var craftingSkillTypeList = skillStatData.CraftingSkillTypeList;
        count = craftingSkillTypeList.Count;
        for (int i = 0; i < count; ++i)
        {
            var createSkillSlot = Instantiate(uISkillSlotPrefab, craftingContentPoint);
            createSkillSlot.InitializedInfo(skillStatData.SkillSpriteTable[craftingSkillTypeList[i]], craftingSkillTypeList[i], (int)craftingSkillTypeList[i], (int)craftingSkillTypeList[i]);
            createSkillSlot.onLifeSkillUpEvent.AddListener(OnSkillLevelUp);
            lifeStat.OnChangeSkillLevelCountEvent.AddListener((lifeType, currentLevel, maxLevel) =>
            {
                if (createSkillSlot.LifeSkillType == lifeType)
                {
                    createSkillSlot.OnChangeSkillLevel(currentLevel, skillStatData.SkillStatTable[craftingSkillTypeList[i]].Count);
                }
            });

            createSkillSlot.onClickAction += OnSetDescription;

            if (lifeStat.SkillLevelTable.TryGetValue((SkillType)i, out var value))
            {
                OnChangeSkillLevel((SkillType)i, value, skillStatData.SkillStatTable[craftingSkillTypeList[i]].Count);
            }
        }
    }

    public void OnSkillLevelUp(SkillType lifeSkillType)
    {
        onSkillLevelUpEvent?.Invoke(lifeSkillType);
    }

    public void OnChangeSkillPoint(int skillPoint)
    {
        skillPointText.text = string.Format(skillPointFormat, skillPoint.ToString());
    }

    public void OnResetSkill()
    {
        onResetSkillEvent?.Invoke();

        foreach (var normalText in normalTextList)
        {
            normalText.gameObject.SetActive(false);
        }
    }

    public void OnSetDescription(SkillType lifeSkillType)
    {
        if(currentLifeSkillType == lifeSkillType)
        {
            return;
        }

        skillNameText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType];// DataTableManager.StringTable.Get(nameID);
        skillDescriptionText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType]; // DataTableManager.StringTable.Get(descriptionID);
    }

    public void OnChangeSkillLevel(SkillType lifeSkillType, int value, int maxLevel)
    {
        int index = (int)lifeSkillType;
        normalTextList[index].gameObject.SetActive(true);
        normalTextList[index].text = string.Format(TypeName.LifeSkillTypeNameFormat[index], (valueList[index]).ToString());
    }
    //public void OnChangeSkillLevel(LifeSkillType lifeSkillType, int currentLevel, int maxLevel)
    //{

    //}
}
