using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayerStrongpointEvent : MonoBehaviour
{
    [SerializeField]
    private Color destroyColor;
    
    public void OnDestroyStrongpoint()
    {
        GetComponent<Renderer>().material.color = destroyColor;
    }

    public void OnResurrection()
    {
        GetComponent<Renderer>().material.color = Color.white;

        var stats = GetComponent<StructureStats>();
        var hpStat = stats.GetStat(StatType.HP);
        stats.OnRepair(hpStat.MaxValue);
    }
}
