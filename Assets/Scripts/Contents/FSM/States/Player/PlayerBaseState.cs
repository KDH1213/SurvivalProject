using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : BaseState<PlayerStateType>
{
    protected PlayerFSM playerFSM;
    protected PlayerStats PlayerStats { get; private set; }

    public PlayerFSM PlayerFSM { get { return playerFSM; } }

    protected virtual void Awake()
    {
        playerFSM = GetComponent<PlayerFSM>();
        PlayerStats = GetComponent<PlayerStats>();
    }
    public override void Enter()
    {
        enterStateEvent?.Invoke();
        this.enabled = true;
    }
    public override void ExecuteUpdate()
    {
        executeUpdateStateEvent?.Invoke();
    }
    public override void ExecuteFixedUpdate()
    {
        executeFixedUpdateStateEvent?.Invoke();
    }

    public override void Exit()
    {
        exitStateEvent?.Invoke();
        this.enabled = false;
    }
}
