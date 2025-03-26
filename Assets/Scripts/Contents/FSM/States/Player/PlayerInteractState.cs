using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractState : PlayerBaseState
{
    private GameObject target;

    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Interact;
    }

    public override void Enter()
    {
        playerFSM.useMove = false;
        Debug.Log("CurrentState is Interact!");
        InteractObject();
    }

    public override void ExecuteUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerFSM.useMove = true;
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public override void Exit()
    {
        //playerFSM.ChangeState(PlayerStateType.Idle);
    }

    public void InteractObject()
    {
        if (playerFSM.Target == null)
        {
            Debug.LogError("target이 null입니다! 상호작용할 대상이 없습니다.");
            return;
        }

        var targetComponent = playerFSM.Target.GetComponent<TestObject>();
        if (targetComponent == null)
        {
            Debug.LogError("target에 TestObject 컴포넌트가 없습니다!");
            return;
        }

        Debug.Log($"상호작용: {targetComponent.name}");
    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;

        // 타겟 별 애니메이션 호출

    }
}
