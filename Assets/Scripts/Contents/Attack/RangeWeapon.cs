using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon.asset", menuName = "Attack/RangeWeapon")]
public class RangeWeapon : Weapon
{
    [SerializeField]
    private Vector3 createOffset;

    public override void Execute(GameObject attacker, GameObject defender)
    {
        if (defender == null)
        {
            return;
        }

        var projectile = Instantiate(weaponPrefab, attacker.transform.forward + createOffset, attacker.transform.rotation);
        var projectileable = projectile.GetComponent<IProjectileable>();
        projectileable.Shot(attacker, defender);
    }
}
