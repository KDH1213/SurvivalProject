using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Interact;
    }

    public override void Enter()
    {
        playerFSM.isMove = false;
        Debug.Log("CurrentState is Interact!");
        InteractObject();
    }

    public override void ExecuteUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerFSM.isMove = true;
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public override void Exit()
    {
        //playerFSM.ChangeState(PlayerStateType.Idle);
    }

    public void InteractObject()
    {
        if (playerFSM.target == null)
        {
            Debug.LogError("arget이 null입니다! 상호작용할 대상이 없습니다.");
            return;
        }

        var targetComponent = playerFSM.target.GetComponent<TestObject>();
        if (targetComponent == null)
        {
            Debug.LogError("target에 TestObject 컴포넌트가 없습니다!");
            return;
        }

        Debug.Log($"상호작용: {targetComponent.name}");
    }
}
