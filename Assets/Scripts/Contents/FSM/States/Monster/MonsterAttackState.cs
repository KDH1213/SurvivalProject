using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    private Coroutine attackCoroutine;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Attack;
    }

    public override void Enter()
    {
        Debug.Log("Monster: Attack State!!");
        MonsterFSM.SetIsAttack(true);
        MonsterFSM.SetIsChase(false);
        attackCoroutine = StartCoroutine(CoAttack());
    }

    public override void ExecuteUpdate()
    {
        if(!MonsterFSM.isAttack)
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
        if(MonsterFSM.CurrentStateType == MonsterStateType.Attack)
        {
            MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.Target.position);

            if(MonsterFSM.TargetDistance > 1.0f)
            {
                MonsterFSM.SetIsAttack(false);
            }
        }
    }

    private IEnumerator CoAttack()
    {
        while(MonsterFSM.isAttack)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);

            yield return waitForSeconds;
        }
    }
}
