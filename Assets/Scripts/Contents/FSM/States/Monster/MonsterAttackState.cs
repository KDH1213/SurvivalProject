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
        MonsterFSM.Animator.speed = animationSpeed;

        var targetPosition = MonsterFSM.Target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);

        Agent.isStopped = true;
        Agent.speed = 0f;
        Agent.destination = transform.position;
        Agent.velocity = Vector3.zero;

        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, MonsterFSM.Agent.speed);
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
    }

    public void OnEndAttackAnimationMonster()
    {
        if(MonsterFSM.CurrentStateType != stateType)
        {
            return;
        }

        if(MonsterFSM.Target == null)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
            return;
        }
        else
        {
            var targetDistance = MonsterFSM.Target.transform.position - MonsterFSM.transform.position;
            targetDistance.y = 0f;
            if(targetDistance.sqrMagnitude > MonsterFSM.Weapon.Range * MonsterFSM.Weapon.Range)
            {
                MonsterFSM.ChangeState(MonsterStateType.Chase);
            }
            else
            {
                MonsterFSM.ChangeState(MonsterStateType.Attack);
            }
            
        }
    }
    public void OnStartAttack()
    {
        MonsterFSM.Weapon.StartAttack(MonsterFSM.AttackPoint, gameObject);

        var targetStats = MonsterFSM.Target.GetComponent<CharactorStats>();
        if (targetStats != null && targetStats.IsDead)
        {
            MonsterFSM.Target = null;
            MonsterFSM.TargetTransform = null;
        }
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
