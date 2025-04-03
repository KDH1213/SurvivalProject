using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedAct : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, DamageInfo damageInfo)
    {
        attacker.GetComponent<IAct>().PlayAct((int)ActType.Attack);
    }
}
