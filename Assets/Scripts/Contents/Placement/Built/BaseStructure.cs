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
        // ���� �ı� ���� ó��
    }
}
