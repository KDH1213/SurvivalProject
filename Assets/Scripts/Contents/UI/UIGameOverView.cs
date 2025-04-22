using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIGameOverView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private TextMeshProUGUI tipText;

    [SerializeField]
    private Button restartButton;
    
    [SerializeField]
    private Button resurrectionButton;

    [SerializeField]
    private GameObject gameoverPanel;

    private bool isSelete = false;

    private void Awake()
    {
        var stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();

        resurrectionButton.onClick.AddListener(() => { gameoverPanel.SetActive(false); isSelete = true; });

        restartButton.onClick.AddListener(stageManager.OnRestart);
        restartButton.onClick.AddListener(() => { gameoverPanel.SetActive(false); isSelete = true; });
    }

    private void OnDisable()
    {
        if (isSelete)
        {
            isSelete = false;
        }
        else
        {
#if !UNITY_EDITOR
            SaveLoadManager.Data.ResetStageInfo();
#endif
        }
    }

    public void SetGameOverText(string text)
    {
        gameOverText.text = text;
    }
    public void SetGameOverText(int id)
    {
        gameOverText.text = DataTableManager.StringTable.Get(id);
    }

    public void SetTipText(string text)
    {
        tipText.text = text;
    }
    public void SetTipText(int id)
    {
        tipText.text = DataTableManager.StringTable.Get(id);
    }

    public void SetResurrectionAction(UnityAction resurrectionAction)
    {
        resurrectionButton.onClick.AddListener(resurrectionAction);
    }
}
