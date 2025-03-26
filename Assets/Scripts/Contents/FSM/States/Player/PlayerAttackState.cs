using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    [SerializeField]
    private LayerMask attackTargetLayerMask;

    private Collider[] attackTargets = new Collider[5];

    private bool isChangeMove = true;

    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Attack;
    }

    public override void Enter()
    {
        PlayerFSM.SetCanAttack(false);
        FindTarget();

        playerFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);
        isChangeMove = true;
    }

    public override void ExecuteUpdate()
    {
        if (isChangeMove && PlayerFSM.MoveValue.sqrMagnitude > 0f)
        {
            PlayerFSM.ChangeState(PlayerStateType.Move);
        }
    }

    public override void Exit()
    {
        PlayerFSM.SetCanAttack(true);
        PlayerFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
    }

    // TODO :: Resources/Animation/PlayerAttackAnim 애니메이션 이벤트에 연결
    public void OnEndAttackAnimationPlayer()
    {
        if (playerFSM.CurrentStateType == PlayerStateType.Attack)
        {
            playerFSM.OnEndAttack();
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    // TODO :: Resources/Animation/PlayerAttackAnim 애니메이션 이벤트에 연결
    public void OnAttackPlayer()
    {
        isChangeMove = false;

        if (PlayerFSM.Weapon != null)
        {
            PlayerFSM.Weapon.Execute(gameObject, PlayerFSM.Target);
        }
    }

    private void FindTarget()
    {
        if (PlayerFSM.Target != null)
        {
            var currentTarget = PlayerFSM.Target.GetComponent<MonsterFSM>();
            if (currentTarget == null || currentTarget.IsDead)
            {
                Debug.Log($"Player: {PlayerFSM.Target.name} 사망! 타겟 해제");
                PlayerFSM.Target = null;
            }
            else
            {
                return;
            }
        }

        // TODO :: 충돌체크 수정 할 예정
        int index = Physics.OverlapSphereNonAlloc(transform.position, PlayerFSM.attackRange, attackTargets, attackTargetLayerMask);

        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;
        Vector3 forward = transform.forward; // 플레이어가 바라보는 방향

        for (int i = 0; i < index; ++i)
        {
            if (attackTargets[i] == null) break;

            var target = attackTargets[i].GetComponent<MonsterFSM>();
            if (target == null || target.IsDead) continue;

            Vector3 directionToTarget = (attackTargets[i].transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, attackTargets[i].transform.position);

            // 전방 90도(±45도) 범위 안에 있는지 확인
            if (Vector3.Dot(forward, directionToTarget) > Mathf.Cos(Mathf.Deg2Rad * 45))
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = attackTargets[i].gameObject;
                }
            }
        }

        if (closestTarget != null)
        {
            PlayerFSM.Target = closestTarget;
            Debug.Log($"Player: {PlayerFSM.Target.name} 발견! 가장 가까운 타겟 설정 완료");
        }
        else
        {
            Debug.Log("Player: 타겟을 찾지 못함");
        }
    }
}
