using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    protected float animationSpeed;
    protected GameObject attacker = null;

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
        MonsterFSM.Animator.speed = animationSpeed;

        var targetPosition = MonsterFSM.Target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);

        Agent.isStopped = true;
        Agent.speed = 0f;
        Agent.destination = transform.position;
        Agent.velocity = Vector3.zero;

        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.speed);
        MonsterFSM.Animator.SetTrigger(MonsterAnimationHashCode.hashAttack);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {
        Agent.isStopped = false;
        Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.speed = 1f;
        attacker = null;

        MonsterFSM.OnEndAttack();
    }

    public virtual void OnEndAttackAnimationMonster()
    {
        if(MonsterFSM.CurrentStateType != stateType)
        {
            return;
        }

        MonsterFSM.SetIsCanAttackCancellation(true);

        if (attacker != null && attacker.activeSelf && attacker != MonsterFSM.Target)
        {
            MonsterFSM.Target = attacker;
        }

        MonsterFSM.ChangeState(MonsterStateType.Chase);
    }
    public virtual void OnStartAttack()
    {
        MonsterFSM.Weapon.StartAttack(MonsterFSM.AttackPoint, gameObject);
        MonsterFSM.SetIsCanAttackCancellation(false);

        if (MonsterFSM.Target != null)
        {
            var targetStats = MonsterFSM.Target.GetComponent<CharactorStats>();
            if (targetStats != null && targetStats.IsDead)
            {
                MonsterFSM.Target = null;
            }
        }
    }

    public void OnSetAttacker(GameObject attacker)
    {
        this.attacker = attacker;
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
