using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        animationSpeed = MonsterFSM.animationSpeed;

        Debug.Log("Monster: Attack State!!");
        MonsterFSM.SetIsAttack(true);
        MonsterFSM.SetIsChase(false);

        MonsterFSM.Animator.SetFloat("attackSpeed", animationSpeed);

        StartCoroutine(CoAttack());
    }

    public override void ExecuteUpdate()
    {
        if (!MonsterFSM.isAttack)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
            MonsterFSM.ChangeState(MonsterStateType.Idle);
        }
    }

    public override void Exit()
    {
    }

    public void OnEndAttackAnimationMonster()
    {
        MonsterFSM.Animator.SetFloat("attackSpeed", animationSpeed);

        if (MonsterFSM.CurrentStateType == MonsterStateType.Attack)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);

            MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

            if (MonsterFSM.TargetDistance > 1.0f)
            {
                MonsterFSM.SetIsAttack(false);
            }
        }
    }

    private IEnumerator CoAttack()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(animationSpeed);

        while (MonsterFSM.isAttack)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);

            yield return waitForSeconds;
        }
    }

    public void OnMonsterAttack()
    {
        if (MonsterFSM.Target != null && MonsterFSM.Weapon != null)
        {
            MonsterFSM.Weapon.Execute(gameObject, MonsterFSM.Target);
        }
    }
}
