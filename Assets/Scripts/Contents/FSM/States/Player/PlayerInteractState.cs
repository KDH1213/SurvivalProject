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
        playerFSM.OnSetUseMove(false);
        Debug.Log("CurrentState is Interact!");
        InteractObject();
    }

    public override void ExecuteUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerFSM.OnSetUseMove(true);
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }

    public override void Exit()
    {

    }

    public void InteractObject()
    {
        if(target != null)
        {
            Debug.Log($"Player: Interact {target.name}");
        }

    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;

        // 타겟 별 애니메이션 호출

    }
}
