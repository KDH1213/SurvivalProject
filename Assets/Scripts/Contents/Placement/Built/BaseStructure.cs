using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : PlacementObject
{

    public override void SetData()
    {

    }

    public override void OnAttack(GameObject attacker, DamageInfo damageInfo)
    {
        base.OnAttack(attacker, damageInfo);
        // 거점 파괴 이후 처리
    }
}
