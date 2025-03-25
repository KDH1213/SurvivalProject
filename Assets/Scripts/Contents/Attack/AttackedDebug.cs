using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedDebug : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, DamageInfo attack)
    {
        if(attack.critical)
        {
            Debug.Log("크리티컬!");
        }

        Debug.Log($"{attacker.name} => {gameObject.name} : {attack.damage}");
    }
}
