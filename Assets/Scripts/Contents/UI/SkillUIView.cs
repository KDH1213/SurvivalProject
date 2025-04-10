using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SkillUIView : MonoBehaviour, ISaveLoadData
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private Button[] skillButtons;

    [SerializeField]
    private TextMeshProUGUI[] skillTexts;

    public UnityEvent<int> onSeleteEvent;
    public UnityEvent<int> onChangeSkillPoint;
    public UnityEvent onDisableEvent;

    private List<int> skillTypeList = new List<int>();
    
    private void OnDisable()
    {
        onDisableEvent?.Invoke();
    }

    private void Awake()
    {
        if(SaveLoadManager.Data == null)
        {
            OnSetRandomSkillOption();
        }
        else
        {
            Load();
        }

        var stageManager = GameObject.FindGameObjectWithTag("StageManager");
        if (stageManager != null)
        {
            stageManager.GetComponent<StageManager>().onSaveEvent += Save;
        }
    }

    private void OnSetRandomSkillOption()
    {
        if(skillTypeList.Count == 0)
        {
            for (int i = 0; i < (int)LifeSkillType.End; ++i)
            {
                skillTypeList.Add(i);
            }
        }

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

    public void OnChangeSkillPoint(int skillPoint)
    {
        if (skillPoint <= 0)
        {
            skillPoint = 0;
        }
        else
        {
            OnSetRandomSkillOption();
        }
        onChangeSkillPoint?.Invoke(skillPoint);
    }

    public void OnClickButton(int index)
    {
        onSeleteEvent?.Invoke(skillTypeList[index]);
    }

    public void Save()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        SaveLoadManager.Data.skillUiViewSeleteList = skillTypeList;
    }

    public void Load()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        skillTypeList = SaveLoadManager.Data.skillUiViewSeleteList;

        if(skillTypeList.Count == 0)
        {
            OnSetRandomSkillOption();
        }
        else
        {
            for (int i = 0; i < skillTexts.Length; ++i)
            {
                skillTexts[i].text = ((LifeSkillType)skillTypeList[i]).ToString();
            }
        }      
    }
}
