using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIQuestView : MonoBehaviour
{
    [SerializeField]
    private GameObject questPanel;

    [SerializeField]
    private TextMeshProUGUI questNameText;

    [SerializeField]
    private TextMeshProUGUI questDescText;

    [SerializeField]
    private TextMeshProUGUI[] questDescTexts;
    [SerializeField]
    private TextMeshProUGUI[] targetTexts;

    [SerializeField]
    private Button questClearButton;

    [SerializeField]
    private GameObject buttonView;

    private readonly string targetCountFormat = "{0} / {1}";
    private readonly string moveTextFormat = "({0},{1}) 로 이동하세요";

    public void SetClickCompensationAction(UnityAction compensationAction)
    {
        questClearButton.onClick.AddListener(compensationAction);
    }

    public void SetQuestInfo(QuestData questData)
    {
        questNameText.text = DataTableManager.StringTable.Get(questData.NameID);

        var questInfoList = questData.questInfoList;
        var questInfoCount = questInfoList.Count;

        if(questInfoList[0].questType == QuestType.Movement)
        {
            targetTexts[0].text = string.Format(moveTextFormat, questData.QuestAreaX, questData.QuestAreaZ);
            questDescTexts[0].text = DataTableManager.StringTable.Get(questInfoList[0].descriptID);
        }

        for(int i = 0; i < questInfoCount; ++i)
        {
            targetTexts[i].gameObject.SetActive(true);
            questDescTexts[i].gameObject.SetActive(true);
            questDescTexts[i].text = DataTableManager.StringTable.Get(questInfoList[i].descriptID);
        }
        for (int i = questInfoCount; i < targetTexts.Length; ++i)
        {
            questDescTexts[i].gameObject.SetActive(false);
            targetTexts[i].gameObject.SetActive(false);
        }
        
        OpenQuestView();
    }

    public void OnSetTargetCount(int index, int currentCount, int maxCount)
    {
        targetTexts[index].text = string.Format(targetCountFormat, currentCount, maxCount);
    }

    public void OnActiveButtonView()
    {
        buttonView.SetActive(true);
    }

    public void OpenQuestView()
    {
        questPanel.SetActive(true);
    }

    public void CloseQuestView()
    {
        questPanel.SetActive(false);
    }
}
