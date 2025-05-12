using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectileable
{
    [SerializeField]
    private MonsterWeapon weapon;

    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float speed;

    private GameObject owner;
    private GameObject target;

    private Vector3 dircetion;
    private Collider[] colliders;

    private void Awake()
    {
        colliders = new Collider[1];
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    public void Shot(GameObject owner, GameObject target)
    {
        this.owner = owner;
        this.target = target;

        dircetion = (target.transform.position - owner.transform.position).normalized;
    }

    private void Update()
    {
        transform.position += dircetion * (speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.magnitude * 0.5f, colliders, weapon.WeaponLayerMask.value) != 0)
        {
            if (ReferenceEquals(colliders[0].gameObject, target))
            {
                CharactorStats aStats = GetComponent<CharactorStats>();
                CharactorStats dStats = target.GetComponent<CharactorStats>();

                DamageInfo attack = weapon.CreateAttack(aStats, dStats);

                IAttackable[] attackables = target.GetComponents<IAttackable>();

                foreach (var attackable in attackables)
                {
                    attackable.OnAttack(owner, attack);
                }

                Destroy(gameObject);
            }
            else if (!colliders[0].isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (ReferenceEquals(other.gameObject, target))
    //    {
    //        CharactorStats aStats = GetComponent<CharactorStats>();
    //        CharactorStats dStats = target.GetComponent<CharactorStats>();

    //        Attack attack = weapon.CreateAttack(aStats, dStats);

    //        IAttackable[] attackables = target.GetComponents<IAttackable>();

    //        foreach (var attackable in attackables)
    //        {
    //            attackable.OnAttack(owner, attack);
    //        }

    //        Destroy(gameObject);
    //    }
    //}
}
