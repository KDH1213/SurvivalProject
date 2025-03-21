using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorBaseState : BaseState<CharactorStateType>
{
    protected PlayerFSM playerFSM;
    protected PlayerJoyStickInput joyStickInput;

    public PlayerFSM PlayerFSM { get { return playerFSM; } }

    protected virtual void Awake()
    {
        playerFSM = GetComponent<PlayerFSM>();
        joyStickInput = GetComponent<PlayerJoyStickInput>();
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
