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
        if (MonsterFSM.Target == null)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
            return;
        }
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
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (MonsterFSM != null && MonsterFSM.Weapon != null)
        {
            MonsterFSM.Weapon.OnGizmos(MonsterFSM.AttackPoint);
        }
    }
#endif
}
