using UnityEngine;

public interface IAttackable
{
    void OnAttack(GameObject attacker, DamageInfo damageInfo);
}
