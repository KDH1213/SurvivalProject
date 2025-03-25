using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    //DamageInfo OnDamage(DamageInfo damageInfo);
    bool OnDamage(ref DamageInfo inoutDamageInfo);

    bool IsDead {  get; }

    UnityEvent DeathEvent { get; }
}
