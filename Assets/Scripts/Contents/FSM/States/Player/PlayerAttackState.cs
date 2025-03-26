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

    public void OnEndAttackAnimationPlayer()
    {
        if (playerFSM.CurrentStateType == PlayerStateType.Attack)
        {
            playerFSM.OnEndAttack();
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    //TODO: Resources/Animation/PlayerAttackAnim 애니메이션 이벤트에 연결
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
            return;
        }

        // TODO :: 충돌체크 수정 할 예정
        int index = Physics.OverlapSphereNonAlloc(transform.position, PlayerFSM.attackRange, attackTargets, attackTargetLayerMask);

        for (int i = 0; i < index; ++i)
        {
            if (attackTargets[i] == null)
            {
                break;
            }

            var target = attackTargets[i].GetComponent<MonsterFSM>();
            if (target != null && !target.isDead)
            {
                PlayerFSM.Target = attackTargets[i].gameObject;
                Debug.Log($"Player: {PlayerFSM.Target.name} 발견! 타겟 설정 완료");
            }
            else
            {
                Debug.Log("Player: 타겟 설정 불가");
            }
            return;
        }
    }
}
