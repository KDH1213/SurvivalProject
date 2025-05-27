using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISkillLevelText : MonoBehaviour
{
    [SerializeField]
    private GameObject skilllPointPanel;

    [SerializeField]
    private TextMeshProUGUI skillPointText;

    public void OnChangeSkillPoint(int point)
    {
        skillPointText.text = point.ToString();

        if (point <= 0)
        {
            skilllPointPanel.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            skilllPointPanel.SetActive(true);
            gameObject.SetActive(true);
        }
    }
}
