using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    private float animationSpeed;
    //private WaitForSeconds waitForSeconds = new WaitForSeconds(animationSpeed);

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Attack;
    }

    public override void Enter()
    {
        animationSpeed = MonsterStats.AttackSpeed;
        MonsterFSM.Animator.SetFloat("attackSpeed", animationSpeed);
        MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);
        MonsterFSM.Animator.Play(AnimationHashCode.hashAttack, 0, 0f);

        StartCoroutine(CoAttack());
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

    private IEnumerator CoAttack()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(animationSpeed);

        MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);
        bool isAttack = true;
        while (isAttack)
        {
            yield return waitForSeconds;
        }

        MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
        MonsterFSM.ChangeState(MonsterStateType.Idle);
    }

    public void OnMonsterAttack()
    {
        if (MonsterFSM.Target != null && MonsterFSM.Weapon != null)
        {
            MonsterFSM.Weapon.Execute(gameObject, MonsterFSM.Target);
        }
    }
}
