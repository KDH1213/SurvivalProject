using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            var IInteractable = target.GetComponent<IInteractable>();


            IInteractable.Interact(this.gameObject);

            var targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            if (IInteractable.InteractType <= InteractType.Box)
            {
                var IDestructible = target.GetComponent<IDestructible>();
                if (IDestructible != null)
                {
                    IDestructible.OnDestruction(this.gameObject);
                }
            }

            target = null;                    
        }

    }

    public void OnSetTarget(GameObject target)
    {
        this.target = target;

        // 타겟 별 애니메이션 호출

    }
}
