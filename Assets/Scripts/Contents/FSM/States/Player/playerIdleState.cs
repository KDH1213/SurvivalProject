using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Idle;
    }

    private void Start()
    {
    }

    public override void Enter()
    {
    }

    public override void ExecuteUpdate()
    {
        if (PlayerFSM.MoveValue.sqrMagnitude > 0f)
        {
            PlayerFSM.ChangeState(PlayerStateType.Move);
        }
    }

    public override void Exit()
    {
    }

    public void OnChangeStatus()
    {
        // TODO :: player Status ���ۿ� ���� ���� �޾ư� ����
        // ĳ���� ���ݷ�, �нú� ��ų ���� ���� ������ ����
        //attackTime = 1f / PlayerFSM.CharactorData.AttackSpeed;
    }

    private void FindAttackTaget()
    {
        //int count = OverlapCollider.StartOverlapCircle(CharactorFSM.AttackDetectionPoint, charactorFSM.CharactorData.RealAttackRange * 0.5f, hitLayerMasks);
        //if (count == 0)
        //    return;

        //switch (attackData.AttackType)
        //{
        //    case AttackType.Single:
        //    case AttackType.Multiple:
        //        {
        //            var hitTarget = OverlapCollider.HitColliderList;
        //            int targetIndex = FindeTarget(ref hitTarget, count);

        //            if (targetIndex == -1)
        //                return;

        //            ((CharactorAttackState)CharactorFSM.StateTable[CharactorStateType.Attack]).SetAttackTarget(hitTarget[targetIndex]);
        //        }
        //        break;
        //    case AttackType.Area:

        //        ((CharactorAttackState)CharactorFSM.StateTable[CharactorStateType.Attack]).SetAttackTarget(OverlapCollider.HitColliderList[0]);
        //        break;
        //    default:
        //        break;
        //}

        //StartCoroutine(CoAttackReload());
        //CharactorFSM.ChangeState(CharactorStateType.Attack);
    }
}
