using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    [SerializeField]
    private Slider hpBarSlider;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private Slider experienceSlider;
    [SerializeField]
    private TextMeshProUGUI experienceText;

    private readonly string levelFormat = "Lv.{0}";
    private readonly string hpFormat = "{0} / {1}";
    private const string pointFormat = "F0";
    private readonly string experienceFormat = "{0}/{1}";

    private void Awake()
    {
        var player = GameObject.FindWithTag(Tags.Player);
        var playerStats = player.GetComponent<PlayerStats>();
        var playerLifeStats = player.GetComponent<LifeStat>();

        playerLifeStats.onChangeLevelEvent.AddListener(OnChangeLevel);
        playerLifeStats.onChangeExperienceEvent.AddListener(OnChangeExperienceSlider);
        playerStats.onChangeHpEvent.AddListener(OnChangeHp);
    }

    public void OnChangeExperienceSlider(float currentValue, float maxValue)
    {
        experienceSlider.value = currentValue / maxValue;
        experienceText.text = string.Format(experienceFormat, currentValue.ToString(), maxValue.ToString());
    }

    public void OnChangeLevel(int level)
    {
        levelText.text = string.Format(levelFormat, level.ToString());
    }

    public void OnChangeHp(StatValue value)
    {
        hpBarSlider.value = value.Value / value.MaxValue;
        hpText.text = string.Format(hpFormat, value.Value.ToString(pointFormat), value.MaxValue.ToString(pointFormat));
    }
}
