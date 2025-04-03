using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillLevelUpButton : MonoBehaviour
{
    [SerializeField]
    private GameObject levelUpPanel;

    [SerializeField]
    private TextMeshProUGUI skillPointText;

    public void OnChangeSkillPoint(int point)
    {
        skillPointText.text = point.ToString();

        if(point <= 0)
        {
            levelUpPanel.SetActive(false);
            gameObject.SetActive(false);    
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
