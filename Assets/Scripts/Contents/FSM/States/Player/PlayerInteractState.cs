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
            Debug.LogError("target�� null�Դϴ�! ��ȣ�ۿ��� ����� �����ϴ�.");
            return;
        }

        var targetComponent = playerFSM.Target.GetComponent<TestObject>();
        if (targetComponent == null)
        {
            Debug.LogError("target�� TestObject ������Ʈ�� �����ϴ�!");
            return;
        }

        Debug.Log($"��ȣ�ۿ�: {targetComponent.name}");
    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;

        // Ÿ�� �� �ִϸ��̼� ȣ��

    }
}
