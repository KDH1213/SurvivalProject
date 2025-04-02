using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillUIView : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private Button[] skillButtons;

    [SerializeField]
    private TextMeshProUGUI[] skillTexts;

    public UnityEvent<int> onSeleteEvent;

    private List<int> skillTypeList = new List<int>();


    private void Awake()
    {
        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            skillTypeList.Add(i);
        }
    }

    private void OnEnable()
    {
        OnSetRandomSkillOption();
    } 

    private void OnSetRandomSkillOption()
    {
        SuffleSkillType();
    }

    public void SuffleSkillType()
    {
        for (int i = (int)LifeSkillType.End - 1; i >= 0; --i)
        {
            int rand = Random.Range(0, i + 1);
            var index = skillTypeList[i];
            skillTypeList[i] = skillTypeList[rand];
            skillTypeList[rand] = index;
        }

        for (int i = 0; i < skillTexts.Length; ++i)
        {
            skillTexts[i].text = ((LifeSkillType)skillTypeList[i]).ToString();
        }
    }

    public void OnClickButton(int index)
    {
        onSeleteEvent?.Invoke(skillTypeList[index]);
    }
}
