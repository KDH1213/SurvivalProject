//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharactorBaseState : BaseState<CharactorStateType>
//{
//    protected CharactorFSM charactorFSM;
//    public CharactorFSM CharactorFSM { get { return charactorFSM; } }

//    protected virtual void Awake()
//    {
//        charactorFSM = GetComponent<CharactorFSM>();
//    }
//    public override void Enter()
//    {
//        enterStateEvent?.Invoke();
//        this.enabled = true;
//    }
//    public override void ExecuteUpdate()
//    {
//        executeUpdateStateEvent?.Invoke();
//    }
//    public override void ExecuteFixedUpdate()
//    {
//        executeFixedUpdateStateEvent?.Invoke();
//    }

//    public override void Exit()
//    {
//        exitStateEvent?.Invoke();
//        this.enabled = false;
//    }
//}
