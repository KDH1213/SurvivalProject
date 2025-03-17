using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseState<T> : MonoBehaviour
{
    protected T stateType;
    public T StateType { get { return stateType; } }   

    public UnityEvent enterStateEvent;
    public UnityEvent executeUpdateStateEvent;
    public UnityEvent executeFixedUpdateStateEvent;
    public UnityEvent exitStateEvent;

    public abstract void Enter();
    public abstract void ExecuteUpdate();
    public abstract void ExecuteFixedUpdate();

    public abstract void Exit();
}
