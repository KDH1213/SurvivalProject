using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorIdleState : CharactorBaseState
{
    //private AttackData attackData;

    //private OverlapCollider OverlapCollider;

    private bool isAttack;

    private float attackTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Idle;

        //attackData = CharactorFSM.AttackData;
    }

    private void Start()
    {
    }

    public override void Enter()
    {

    }

    public override void ExecuteUpdate()
    {
        if(PlayerFSM.MoveValue.sqrMagnitude > 0f)
        {
            PlayerFSM.ChangeState(CharactorStateType.Move);
        }
    }

    public override void Exit()
    {
    }

    private IEnumerator CoAttackReload()
    {
        isAttack = false;
        yield return new WaitForSeconds(attackTime);
        isAttack = true;
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
