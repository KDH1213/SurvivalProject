using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
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
    }

    public override void ExecuteUpdate()
    {
        if (playerFSM.isMove && PlayerFSM.MoveValue.sqrMagnitude > 0f)
        {
            PlayerFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
            PlayerFSM.ChangeState(PlayerStateType.Move);
        }

        if (playerFSM.IsAttack)
        {
            playerFSM.ChangeState(PlayerStateType.Attack);
        }
    }

    public override void Exit()
    {
        playerFSM.isMove = true;
    }

    public void OnEndAttackAnimationPlayer()
    {
        if (playerFSM.CurrentStateType == PlayerStateType.Attack)
        {
            playerFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
            playerFSM.IsAttack = false;
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    //TODO: Resources/Animation/PlayerAttackAnim 애니메이션 이벤트에 연결
    public void OnAttackPlayer()
    {
        playerFSM.isMove = false;

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

        Collider[] colliders = Physics.OverlapSphere(transform.position, PlayerFSM.attackRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                var target = collider.GetComponent<MonsterFSM>();
                if (target != null && !target.isDead)
                {
                    PlayerFSM.Target = collider.gameObject;
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
}
