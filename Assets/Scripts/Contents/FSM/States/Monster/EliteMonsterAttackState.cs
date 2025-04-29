using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterAttackState : MonsterAttackState
{
    // private List<MonsterAttackPattern> monsterAttackPatternList = new List<MonsterAttackPattern>();
    private int currentAttackIndex = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        if (MonsterFSM.Target == null)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
            return;
        }

        Agent.isStopped = true;
        Agent.speed = 0f;
        Agent.destination = transform.position;
        Agent.velocity = Vector3.zero;

        var targetPosition = MonsterFSM.Target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);


        currentAttackIndex = Random.Range(0, 4);
        // monsterAttackPatternList[currentAttackIndex].Enter();

        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, 0f);
        MonsterFSM.Animator.SetInteger(MonsterAnimationHashCode.hashAttackIndex, currentAttackIndex);
        MonsterFSM.Animator.SetTrigger(MonsterAnimationHashCode.hashAttack);

        //animationSpeed = MonsterStats.AttackSpeed;
        //MonsterFSM.Animator.speed = animationSpeed;
    }

    public override void ExecuteUpdate()
    {
        // monsterAttackPatternList[currentAttackIndex].ExecuteUpdate();
    }

    public override void ExecuteFixedUpdate()
    {
        // monsterAttackPatternList[currentAttackIndex].ExecuteFixedUpdate();
    }

    public override void Exit()
    {
        Agent.isStopped = false;
        Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.speed = 1f;
        attacker = null;
    }
}
