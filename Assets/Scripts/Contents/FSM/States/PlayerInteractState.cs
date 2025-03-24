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
            Debug.LogError("arget�� null�Դϴ�! ��ȣ�ۿ��� ����� �����ϴ�.");
            return;
        }

        var targetComponent = playerFSM.target.GetComponent<TestObject>();
        if (targetComponent == null)
        {
            Debug.LogError("target�� TestObject ������Ʈ�� �����ϴ�!");
            return;
        }

        Debug.Log($"��ȣ�ۿ�: {targetComponent.name}");
    }
}
