using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDurabilityData", menuName = "System/Inventory/ItemDurabilityData", order = 1)]
public class ItemDurabilityData : ScriptableObject
{
    [SerializeField]
    private Color greenGradientColor;
    [SerializeField]
    private Color orangeGradientColor;
    [SerializeField]
    private Color redGradientColor;

    [SerializeField]
    [Range(0.1f,0.99f)]
    private float orangeGradientRange;

    [SerializeField]
    [Range(0, 0.99f)]
    private float redGradientRange;

    public Color GetDurabilityColor(float currentDurabillity, float maxDurabillity)
    {
        float percent = currentDurabillity / maxDurabillity;

        return GetDurabilityColor(percent);
    }

    public Color GetDurabilityColor(float percent)
    {
        if (percent > orangeGradientRange)
        {
            return greenGradientColor;
        }
        else if (percent < orangeGradientRange)
        {
            return orangeGradientColor;
        }
        else
        {
            return redGradientColor;
        }
    }
}
