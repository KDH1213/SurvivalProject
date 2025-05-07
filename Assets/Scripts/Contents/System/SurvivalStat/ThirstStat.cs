using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThirstStat : SurvivalStatBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float startPenaltyPersent = 0.5f;

    [SerializeField]
    private float valueDownTime = 0.5f;

    [SerializeField]
    private Slider thirstSlider;

    [SerializeField]
    private TextMeshProUGUI thirstPersentText;

    private readonly string persentFormat = "{0}%";

    private float currentTime = 0f;

    private float thirstSkillValue = 0f;
    private float totalValueDownTime = 0f;


    [SerializeField]
    private Sprite debuffIcon;

    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Thirst;
        value = MaxValue;
        totalValueDownTime = valueDownTime;

        Load();
        OnChangeValue();

        var uIDebuffIcon = GameObject.FindWithTag(Tags.UIDebuffIcon);
        if (uIDebuffIcon != null)
        {
            var debuff = uIDebuffIcon.GetComponent<UIDebuffIconView>().CreateDebuffIcon(debuffIcon, "갈증 디버프", "피로도 1.5배 증가");
            onStartPenaltyEvent.AddListener((_) => { debuff.gameObject.SetActive(true); });
            onEndPenaltyEvent.AddListener((_) => { debuff.gameObject.SetActive(false); });
            debuff.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= totalValueDownTime)
        {
            currentTime -= totalValueDownTime;
            value -= MaxValue * 0.01f;
            this.value = Mathf.Max(value, 0f);
            OnChangeValue();

            if (!isOnDebuff && IsActivationCheckPenalty())
            {
                OnStartPenalty();
            }
        }
    }

    protected override bool IsActivationCheckPenalty()
    {
        return value < startPenaltyPersent * MaxValue;
    }

    public override void AddPenaltyValue(float value)
    {
        this.value += value;
        this.value = Mathf.Clamp(this.value, 0f, MaxValue);
        OnChangeValue();

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
        else if (isOnDebuff)
        {
            isOnDebuff = IsActivationCheckPenalty();

            if (!isOnDebuff)
            {
                OnStopPenalty();
            }
        }
    }

    public override void SubPenaltyValue(float value)
    {
        this.value -= value;
        this.value = Mathf.Clamp(this.value, 0f, MaxValue);
        OnChangeValue();

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
        else if (isOnDebuff)
        {
            isOnDebuff = IsActivationCheckPenalty();

            if (!isOnDebuff)
            {
                OnStopPenalty();
            }
        }
    }
    public void OnSetThirstSkillValue(float value)
    {
        thirstSkillValue = value;
        totalValueDownTime = valueDownTime + thirstSkillValue;
    }
    public override void OnStartPenalty()
    {
        base.OnStartPenalty();
    }

    public override void OnStopPenalty()
    {
        base.OnStopPenalty();
    }

    public void OnChangeValue()
    {
        thirstSlider.value = value / MaxValue;
        var persent = value / MaxValue;
        thirstSlider.value = persent;
        thirstPersentText.text = string.Format(persentFormat, ((int)(persent * 100f)).ToString());
    }
}
