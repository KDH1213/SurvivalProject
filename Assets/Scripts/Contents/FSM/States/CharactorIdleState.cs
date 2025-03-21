using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorIdleState : CharactorBaseState
{
    [SerializeField]
    protected LayerMask hitLayerMasks;

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
        if (isAttack)
        {
            FindAttackTaget();
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

    //private int FindeTarget(ref List<Collider2D> hitTargeList, int count)
    //{
    //    var routeMove = hitTargeList[0].GetComponent<IRouteMove>();
    //    var damageable = hitTargeList[0].GetComponent<IDamageable>();
    //    int targetIndex = 0;

    //    if (damageable.IsDead)
    //    {
    //        targetIndex = -1;
    //    }

    //    for (int i = 1; i < count; ++i)
    //    {
    //        var targetDamageable = hitTargeList[i].GetComponent<IDamageable>();

    //        if (targetDamageable.IsDead)
    //            continue;
    //        else if (targetIndex == -1)
    //        {
    //            targetIndex = i;
    //            continue;
    //        }

    //        var target = hitTargeList[i].GetComponent<IRouteMove>();
    //        if (routeMove.MoveIndex == target.MoveIndex)
    //        {
    //            if (routeMove.MoveDirection.x == 0f)
    //            {
    //                if ((routeMove.MoveDirection.y > 0f && routeMove.CurrentPosition.y < target.CurrentPosition.y)
    //                    || routeMove.MoveDirection.y < 0f && routeMove.CurrentPosition.y > target.CurrentPosition.y)
    //                {
    //                    routeMove = target;
    //                    targetIndex = i;
    //                }
    //            }
    //            else
    //            {
    //                if ((routeMove.MoveDirection.x > 0f && routeMove.CurrentPosition.x < target.CurrentPosition.x)
    //                      || routeMove.MoveDirection.x < 0f && routeMove.CurrentPosition.x > target.CurrentPosition.x)
    //                {
    //                    routeMove = target;
    //                    targetIndex = i;
    //                }
    //            }
    //        }
    //        else if ((routeMove.MoveIndex > 0 && target.MoveIndex == 0) || routeMove.MoveIndex < target.MoveIndex)
    //        {
    //            routeMove = target;
    //            targetIndex = i;
    //        }
    //    }

    //    return targetIndex;
    //}


    public void OnChangeStatus()
    {
        // TODO :: player Status 제작에 따라 값을 받아게 변경
        // 캐릭터 공격력, 패시브 스킬 적용 여부 미정인 상태
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
