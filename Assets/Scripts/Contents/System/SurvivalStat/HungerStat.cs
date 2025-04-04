using System.Collections;
using System.Collections.Generic;
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

    private LifeStat lifeStat;

    private bool isHpDown;
    private float currentTime = 0f;

    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Hunger;
        value = maxValue;

        lifeStat = GetComponent<LifeStat>();
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
        if (currentTime >= valueDownTime)
        {
            currentTime -= valueDownTime;
            value -= maxValue * 0.01f;
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
        return value < maxValue * startPenaltyPersent;
    }

    public override void AddPenaltyValue(float value)
    {
        this.value += value;

        this.value = Mathf.Clamp(this.value, 0f, maxValue);
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
        hungerSlider.value = value / maxValue;
    }
}
