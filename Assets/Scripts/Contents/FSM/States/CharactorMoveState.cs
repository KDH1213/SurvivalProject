//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharactorMoveState : CharactorBaseState
//{
//    [SerializeField] 
//    private Vector2 movePoint;

//    private Vector2 currentPosition;
//    private Vector2 direction;

//    protected override void Awake()
//    {
//        base.Awake();
//        stateType = CharactorStateType.Move;
//    }

//    public override void Enter()
//    {
//        //currentPosition = monsterTransform.position;
//        //moveSpeed = charactorFSM.GetStatValue(StatType.MovementSpeed);
//        //GetMovePoint();
//        if (charactorFSM.Animator != null)
//            charactorFSM.Animator.SetBool(DHUtil.CharactorAnimationUtil.hashIsMove, true);

//        if(direction.x < 0f && charactorFSM.IsFlip())
//        {
//            charactorFSM.OnFlip();
//        }
//        else if (direction.x > 0f && !charactorFSM.IsFlip())
//        {
//            charactorFSM.OnFlip();
//        }
//    }

//    public override void ExecuteUpdate()
//    {
//        Move();
//    }

//    public override void Exit()
//    {
//        if (charactorFSM.Animator != null)
//            charactorFSM.Animator.SetBool(DHUtil.CharactorAnimationUtil.hashIsMove, false);
//    }

//    public void Move()
//    {
//        currentPosition += direction * (Time.deltaTime * CharactorFSM.CharactorProfile.MoveSpeed);
//        transform.position = currentPosition;

//        if (IsCheckResult())
//        {
//            transform.position = movePoint;
//            CharactorFSM.ChangeState(CharactorStateType.Idle);
//        }
//    }

//    private void OnFilp()
//    {
//    }

//    public void OnSetMovePoint(Vector2 movePoint)
//    {
//        currentPosition = transform.position;
//        this.movePoint = movePoint;

//        direction = movePoint - currentPosition;
//        direction.Normalize();
//    }

//    public bool IsCheckResult()
//    {
//        Vector2 position;

//        if (direction.x > 0f)
//            position.x = Mathf.Min(movePoint.x, currentPosition.x);
//        else
//            position.x = Mathf.Max(movePoint.x, currentPosition.x);

//        if (direction.y > 0f)
//            position.y = Mathf.Min(movePoint.y, currentPosition.y);
//        else
//            position.y = Mathf.Max(movePoint.y, currentPosition.y);

//        if (movePoint == position)
//            return true;

//        return false;
//    }
//}
