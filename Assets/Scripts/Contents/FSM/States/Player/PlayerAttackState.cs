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

    // TODO :: Resources/Animation/PlayerAttackAnim �ִϸ��̼� �̺�Ʈ�� ����
    public void OnEndAttackAnimationPlayer()
    {
        if (playerFSM.CurrentStateType == PlayerStateType.Attack)
        {
            playerFSM.OnEndAttack();
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    // TODO :: Resources/Animation/PlayerAttackAnim �ִϸ��̼� �̺�Ʈ�� ����
    public void OnAttackPlayer()
    {
        isChangeMove = false;
        PlayerFSM.Weapon.StartAttack(transform);
    }

    private void FindTarget()
    {
        int index = Physics.OverlapSphereNonAlloc(transform.position, PlayerFSM.attackRange, attackTargets, attackTargetLayerMask);
        
        foreach(var attackTargets in PlayerFSM.AttackTargets)
        {
            if(attackTargets != null)
            {
                MonsterFSM currentTarget = attackTargets.GetComponent<MonsterFSM>();
                if(currentTarget == null || currentTarget.IsDead)
                {
                    PlayerFSM.AttackTargets.Remove(attackTargets);
                }
            }
        }

        
        // TODO :: ���� Ÿ�ٿ��� ���� Ÿ������ ����

        Vector3 forward = transform.forward; // �÷��̾ �ٶ󺸴� ����

        for (int i = 0; i < index; ++i)
        {
            if (attackTargets[i] == null)
            {
                break;
            }

            MonsterFSM target = attackTargets[i].GetComponent<MonsterFSM>();
            if (target == null || target.IsDead)
            {
                continue;
            }

            Vector3 directionToTarget = (attackTargets[i].transform.position - transform.position).normalized;

            // ���� 90��(��45��) ���� �ȿ� �ִ��� Ȯ��
            if (Vector3.Dot(forward, directionToTarget) > Mathf.Cos(Mathf.Deg2Rad * 45))
            {
                PlayerFSM.AttackTargets.Add(attackTargets[i].gameObject);
            }
        }
    }
}
