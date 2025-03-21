
[System.Serializable]
public struct IntMinMax
{
    public int min;
    public int max;

    public static IntMinMax GetValue(int min, int max)
    {
        var value = new IntMinMax();
        value.min = min;
        value.max = max;
        return value;
    }

    public int GetRendomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}

[System.Serializable]
public struct FloatMinMax
{
    public float min;
    public float max;

    public void SetValue(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public static FloatMinMax GetValue(float min, float max)
    {
        var value = new FloatMinMax();
        value.min = min;
        value.max = max;
        return value;
    }

    public float GetRendomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}
