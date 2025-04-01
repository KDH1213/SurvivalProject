using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    private float animationSpeed;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Attack;
    }

    public override void Enter()
    {
        animationSpeed = MonsterStats.AttackSpeed;
        MonsterFSM.Animator.SetFloat("attackSpeed", animationSpeed);

        transform.LookAt(MonsterFSM.Target.transform.position);

        MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);
        MonsterFSM.Animator.Play(AnimationHashCode.hashAttack, 0, 0f);

        MonsterFSM.Agent.isStopped = true;
        MonsterFSM.Agent.destination = transform.position;
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {
        MonsterFSM.Agent.isStopped = false;
    }

    public void OnEndAttackAnimationMonster()
    {
        MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
        MonsterFSM.ChangeState(MonsterStateType.Chase);
    }
    public void OnMonsterAttack()
    {
        MonsterFSM.Weapon.StartAttack(MonsterFSM.AttackPoint, gameObject);
        //int index = Physics.OverlapSphereNonAlloc(transform.position, MonsterFSM.Weapon.Range , MonsterFSM.Weapon.AttackTargets, MonsterFSM.Weapon.WeaponLayerMask);

        //for (int i = 0; i < index; ++i)
        //{
        //    var target = MonsterFSM.Weapon.AttackTargets[i].GetComponent<CharactorStats>();
        //    if (target != null)
        //    {
        //        MonsterFSM.Weapon.Execute(gameObject, target.gameObject);
        //    }
        //}

        //if (MonsterFSM.Target != null && MonsterFSM.Weapon != null)
        //{
        //    MonsterFSM.Weapon.Execute(gameObject, MonsterFSM.Target);
        //}
    }

    private void OnDrawGizmos()
    {
        if (MonsterFSM != null && MonsterFSM.Weapon != null)
        {
            MonsterFSM.Weapon.OnGizmos(MonsterFSM.AttackPoint);
        }
    }
}
