using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatValue
{
    [SerializeField]
    private StatType statusInfoType;

    public StatType StatusInfoType { get { return statusInfoType; } }
    [SerializeField]
    private float value;
    public float Value { get { return value; } }

    [SerializeField]
    public FloatMinMax minMaxValue;
    public float MaxValue { get { return minMaxValue.max; } }
    public float MinValue { get { return minMaxValue.min; } }

    public System.Action<float> OnChangeValue;

    public void ValueCopy(StatValue statusValue)
    {
        this.minMaxValue.max = statusValue.MaxValue;
        this.value = statusValue.Value > this.minMaxValue.max ? this.minMaxValue.max : statusValue.Value;
        OnChangeValue?.Invoke(value);
    }

    public float AddValue(float addValue)
    {
        this.value += addValue;
        this.value = Mathf.Clamp(value, minMaxValue.min, minMaxValue.max);
        OnChangeValue?.Invoke(value);

        return this.value;
    }

    public float AddMaxValue(float addValue, bool useAddValue = true)
    {
        this.minMaxValue.max += addValue;

        if (useAddValue)
        {
            AddValue(addValue);
        }

        return this.value;
    }

    public float SetMaxValue(float value)
    {
        minMaxValue.max = value;

        return minMaxValue.max;
    }

    public void OnValueincrease(StatValue statusValue)
    {
        OnValueincrease(statusValue.MaxValue);
    }

    public void OnValueincrease(float value)
    {
        this.minMaxValue.max += value;
        this.value = this.minMaxValue.max;
    }

    public void SetValue(float _value)
    {
        this.value = _value;
        this.minMaxValue.max = System.Math.Max(_value, minMaxValue.max);
        OnChangeValue?.Invoke(value);
    }

    public StatValue(StatType statusInfoType, float value = 0f, float maxValue = 0f, float minValue = 0f)
    {
        this.statusInfoType = statusInfoType;
        this.value = value;
        this.minMaxValue.max = maxValue;
        this.minMaxValue.min = minValue;
    }

    public StatValue(StatValue statValue)
    {
        this.statusInfoType = statValue.statusInfoType;
        this.value = statValue.value;
        this.minMaxValue.max = statValue.MaxValue;
        this.minMaxValue.min = statValue.MinValue;
    }

    public void OnChangeValueAction(System.Action<float> action)
    {
        OnChangeValue += action;
    }

    public void OnActionChangeValue()
    {
        OnChangeValue?.Invoke(value);
    }
}
