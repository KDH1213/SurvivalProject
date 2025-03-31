using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool isChangeMove = true;

    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Attack;
    }

    public override void Enter()
    {
        PlayerFSM.AttackTargets.Clear();
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

        foreach (var attackTargets in PlayerFSM.AttackTargets)
        {
            if (PlayerFSM.Weapon != null)
            {
                PlayerFSM.Weapon.Execute(gameObject, attackTargets);
            }
        }
    }

    private void FindTarget()
    {
        int index = Physics.OverlapSphereNonAlloc(transform.position, PlayerFSM.attackRange, PlayerFSM.Weapon.attackTargets, PlayerFSM.Weapon.weaponLayerMask);

        //int boxIndex = Physics.OverlapBoxNonAlloc(transform.forward + PlayerFSM.PlayerData.offset, PlayerFSM.PlayerData.attackSize, PlayerFSM.Weapon.attackTargets, transform.rotation, PlayerFSM.Weapon.weaponLayerMask);


        // TODO :: 단일 타겟에서 다중 타겟으로 수정

        for (int i = 0; i < index; ++i)
        {
            if (PlayerFSM.Weapon.attackTargets[i] == null)
            {
                break;
            }

            MonsterFSM target = PlayerFSM.Weapon.attackTargets[i].GetComponent<MonsterFSM>();
            if (target == null || target.IsDead)
            {
                continue;
            }

            PlayerFSM.AttackTargets.Add(PlayerFSM.Weapon.attackTargets[i].gameObject);
        }
    }
}
