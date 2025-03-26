using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    private CharactorStats charactorStats;

    private void Awake()
    {
        charactorStats = GetComponent<CharactorStats>();
    }

    public void OnAttack(GameObject attacker, DamageInfo attack)
    {
        var hpStat = charactorStats.GetStat(StatType.HP);
        hpStat.AddValue(-attack.damage);
        Debug.Log($"Damage: {attack.damage} / Hp: {charactorStats.GetStatValue(StatType.HP)}");

        charactorStats.damegedEvent?.Invoke();

        if (hpStat.Value <= 0)
        {
            Debug.Log("Die!");
            IDestructible[] destructibles = GetComponents<IDestructible>();

            foreach(IDestructible destructible in destructibles)
            {
                destructible.OnDestruction(attacker);
            }
        }
    }
}
