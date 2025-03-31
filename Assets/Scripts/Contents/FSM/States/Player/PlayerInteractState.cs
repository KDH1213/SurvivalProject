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
        InteractObject();
        playerFSM.ChangeState(PlayerStateType.Idle);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }

    public void InteractObject()
    {
        if(target != null)
        {
            Destroy(target);
            target = null;
        }

    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;

        // Ÿ�� �� �ִϸ��̼� ȣ��

    }
}
