using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuestView : MonoBehaviour
{
    [SerializeField]
    private GameObject questPanel;

    [SerializeField]
    private TextMeshProUGUI questNameText;

    [SerializeField]
    private TextMeshProUGUI questDescText;

    [SerializeField]
    private TextMeshProUGUI targetText;

    private readonly string targetCountFormat = "{0} / {1}";
    private readonly string moveTextFormat = "({0},{1}) 로 이동하세요";

    public void SetQuestInfo(QuestData questData)
    {
        questNameText.text = DataTableManager.StringTable.Get(questData.NameID);
        questDescText.text = DataTableManager.StringTable.Get(questData.DescriptID);

        var questInfoList = questData.questInfoList;
        var questInfoCount = questInfoList.Count;

        if(questInfoList[0].questType == QuestType.Movement)
        {
            targetText.text = string.Format(moveTextFormat, questData.QuestAreaX, questData.QuestAreaZ);
        }
        OpenQuestView();
    }

    public void OnSetTargetCount(int index, int currentCount, int maxCount)
    {
        targetText.text = string.Format(targetCountFormat, currentCount, maxCount);
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
