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

    public UnityEvent<NormalSkillType> onSkillLevelUpEvent;
    public UnityEvent onResetSkillEvent;

    private NormalSkillType currentLifeSkillType = NormalSkillType.End;

    private string normalText;
    private string lifeText;
    private string craftingText;
    private readonly string skillPointFormat = "스킬 포인트 : {0}";

    private void OnDisable()
    {
        currentLifeSkillType = NormalSkillType.End;
    }

    private void Awake()
    {
        var lifeStat = GameObject.FindWithTag(Tags.Player).GetComponent<LifeStat>();
        onSkillLevelUpEvent.AddListener(lifeStat.OnSkillLevelUp);
        lifeStat.OnChangeSkillLevelCountEvent.AddListener(OnChangeSkillLevel);
        lifeStat.OnChangeSkillPointEvent.AddListener(OnChangeSkillPoint);
        onResetSkillEvent.AddListener(lifeStat.OnResetSkill);
        OnChangeSkillPoint(lifeStat.SkillPoint);

        valueList = lifeStat.SkillStatValueList;

        var skillData = lifeStat.SkillStatData;

        for (int i = 0; i < (int)NormalSkillType.End; ++i)
        {
            var skillType = (NormalSkillType)i;

            var createSkillSlot = Instantiate(uISkillSlotPrefab, noramlContentPoint);
            createSkillSlot.InitializedInfo(skillData.NormalSkillSpriteTable[skillType], skillType, i, i);
            createSkillSlot.onLifeSkillUpEvent.AddListener(OnSkillLevelUp);
            lifeStat.OnChangeSkillLevelCountEvent.AddListener((lifeType, currentLevel, maxLevel) =>
            {
                if(createSkillSlot.LifeSkillType == lifeType)
                {
                    createSkillSlot.OnChangeSkillLevel(currentLevel, maxLevel);
                }
            });

            createSkillSlot.onClickAction += OnSetDescription;

            if (lifeStat.NormalSkillLevelTable.TryGetValue(skillType, out var value))
            {
                OnChangeSkillLevel((NormalSkillType)i, value, skillData.NormalSkillStatTable[skillType].Count);
            }
        }

        //for (int i = 0; i < (int)LifeSkillType.End; ++i)
        //{
        //    var skillType = (LifeSkillType)i;

        //    var createSkillSlot = Instantiate(uISkillSlotPrefab, lifeContentPoint);
        //    createSkillSlot.InitializedInfo(skillData.LifeSkillStatTable[skillType], skillType, i, i);
        //    createSkillSlot.onLifeSkillUpEvent.AddListener(OnSkillLevelUp);
        //    lifeStat.OnChangeSkillLevelCountEvent.AddListener((lifeType, currentLevel, maxLevel) =>
        //    {
        //        if (createSkillSlot.LifeSkillType == lifeType)
        //        {
        //            createSkillSlot.OnChangeSkillLevel(currentLevel, maxLevel);
        //        }
        //    });

        //    createSkillSlot.onClickAction += OnSetDescription;

        //    if (lifeStat.NormalSkillLevelTable.TryGetValue(skillType, out var value))
        //    {
        //        OnChangeSkillLevel((NormalSkillType)i, value, skillData.NormalSkillStatTable[skillType].Count);
        //    }
        //}



        //for (int i = 0; i < (int)LifeSkillType.End; ++i)
        //{
        //    Instantiate(uISkillSlotPrefab, noramlContentPoint);
        //}

        //for (int i = 0; i < (int)LifeSkillType.End; ++i)
        //{
        //    Instantiate(uISkillSlotPrefab, noramlContentPoint);
        //}
    }

    public void OnSkillLevelUp(NormalSkillType lifeSkillType)
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

    public void OnSetDescription(NormalSkillType lifeSkillType)
    {
        if(currentLifeSkillType == lifeSkillType)
        {
            return;
        }

        skillNameText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType];// DataTableManager.StringTable.Get(nameID);
        skillDescriptionText.text = TypeName.LifeSkillTypeName[(int)lifeSkillType]; // DataTableManager.StringTable.Get(descriptionID);
    }

    public void OnChangeSkillLevel(NormalSkillType lifeSkillType, int value, int maxLevel)
    {
        int index = (int)lifeSkillType;
        normalTextList[index].gameObject.SetActive(true);
        normalTextList[index].text = string.Format(TypeName.LifeSkillTypeNameFormat[index], (valueList[index]).ToString());
    }
    //public void OnChangeSkillLevel(LifeSkillType lifeSkillType, int currentLevel, int maxLevel)
    //{

    //}
}
