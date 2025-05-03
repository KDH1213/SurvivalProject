using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HungerStat : SurvivalStatBehaviour
{
    public UnityEvent onHpPenaltyEvnet;

    [SerializeField]
    [Range(0f, 1f)]
    private float startPenaltyPersent = 0.5f;

    [SerializeField]
    private float valueDownTime = 0.5f;
    [SerializeField]
    private float hpDownTime = 5f;

    [SerializeField]
    private Slider hungerSlider;


    [SerializeField]
    private TextMeshProUGUI hungerPersentText;

    private readonly string persentFormat = "{0}%";

    private bool isHpDown;
    private float currentTime = 0f;
    private float hungerSkillValue = 0f;
    private float totalValueDownTime = 0f;

    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Hunger;
        value = MaxValue;
        totalValueDownTime = valueDownTime;
        Load();

        OnChangeValue();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(isOnDebuff)
        {
            if (value <= 0f)
            {
                CheckHpDown();
            }
            else
            {
                CheckValueDown();
            }
        }
        else
        {
            CheckValueDown();
            if (!isOnDebuff && IsActivationCheckPenalty())
            {
                OnStartPenalty();
            }
        }
    }

    private void CheckValueDown()
    {
        if (currentTime >= totalValueDownTime)
        {
            currentTime -= totalValueDownTime;
            value -= MaxValue * 0.01f;
            this.value = Mathf.Max(value, 0f);
           OnChangeValue();
        }
    }

    private void CheckHpDown()
    {
        if (currentTime >= hpDownTime)
        {
            currentTime -= hpDownTime;
            onHpPenaltyEvnet?.Invoke();
        }
    }

    protected override bool IsActivationCheckPenalty()
    {
        return value < MaxValue * startPenaltyPersent;
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

            if(!isOnDebuff)
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

    public void OnSetHungerSkillValue(float value)
    {
        hungerSkillValue = value;
        totalValueDownTime = valueDownTime + hungerSkillValue;
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
        var persent = value / MaxValue;
        hungerSlider.value = persent;
        hungerPersentText.text = string.Format(persentFormat, ((int)(persent * 100f)).ToString());
    }
}
