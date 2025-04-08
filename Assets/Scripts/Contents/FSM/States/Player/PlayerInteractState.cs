using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // Debug.Log($"Player: Interact {target.name}");
            target.GetComponent<IInteractable>().Interact(this.gameObject);

            transform.LookAt(target.transform);
            target = null;
        }

    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;

        // 타겟 별 애니메이션 호출

    }
}
