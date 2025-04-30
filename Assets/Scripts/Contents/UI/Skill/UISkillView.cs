using System.Collections;
using System.Collections.Generic;
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

    public UnityEvent<LifeSkillType> onSkillLevelUpEvent;


    private void Awake()
    {
        var lifeStat = GameObject.FindWithTag(Tags.Player).GetComponent<LifeStat>();
        onSkillLevelUpEvent.AddListener(lifeStat.OnSkillLevelUp);

        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            var createSkillSlot = Instantiate(uISkillSlotPrefab, noramlContentPoint);
            createSkillSlot.InitializedInfo(null, (LifeSkillType)i, i, i);
            createSkillSlot.onLifeSkillUpEvent.AddListener(OnSkillLevelUp);
            lifeStat.OnChangeSkillLevelCountEvent.AddListener((lifeType, currentLevel, maxLevel) =>
            {
                if(createSkillSlot.LifeSkillType == lifeType)
                {
                    createSkillSlot.OnChangeSkillLevel(currentLevel, maxLevel);
                }
            });
        }

        //for (int i = 0; i < (int)LifeSkillType.End; ++i)
        //{
        //    Instantiate(uISkillSlotPrefab, noramlContentPoint);
        //}

        //for (int i = 0; i < (int)LifeSkillType.End; ++i)
        //{
        //    Instantiate(uISkillSlotPrefab, noramlContentPoint);
        //}
    }

    public void OnSkillLevelUp(LifeSkillType lifeSkillType)
    {
        onSkillLevelUpEvent?.Invoke(lifeSkillType);
    }

    //public void OnChangeSkillLevel(LifeSkillType lifeSkillType, int currentLevel, int maxLevel)
    //{

    //}
}
