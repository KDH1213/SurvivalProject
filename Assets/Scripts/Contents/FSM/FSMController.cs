using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FSMController<T> : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Use Start State")]
    protected T currentStateType;
    public T CurrentStateType { get { return currentStateType; } }

    [SerializeField]
    protected T previousStateType;
    public T PreviousStateType { get { return previousStateType; } }

    [SerializedDictionary("StateType", "State"), SerializeField] protected SerializedDictionary<T, BaseState<T>> stateTable = new SerializedDictionary<T, BaseState<T>>();
    public SerializedDictionary<T, BaseState<T>> StateTable { get { return stateTable; } }

    protected virtual void Awake()
    {
    }

    public void StartState()
    {
        stateTable[currentStateType]?.Enter();
    }

    public void ChangeState(T stateType)
    {
        if (currentStateType.GetHashCode() == stateType.GetHashCode() 
            || !stateTable.ContainsKey(stateType))
            return;

        previousStateType = currentStateType;
        currentStateType = stateType;

        stateTable[previousStateType]?.Exit();
        stateTable[currentStateType]?.Enter();
    }

    //protected virtual void Update()
    //{
    //    stateTable[currentStateType]?.ExcuteUpdate();
    //}

    //protected virtual void FixedUpdate()
    //{
    //    stateTable[currentStateType]?.ExcuteFixedUpdate();
    //}

    public virtual void AddState(T stateType, BaseState<T> state)
    {
        if (stateTable.ContainsKey(stateType))
            return;

        stateTable.Add(stateType, state);
    }

    public virtual void AddAllStates()
    {

    }

    public virtual void FindAndAddState(T findStateType)
    {
    }
    public virtual void FindAndAddAllStates()
    {
    }

    public virtual void RemoveState(T stateType) 
    {
        if (!stateTable.ContainsKey(stateType))
            return;

        DestroyImmediate(stateTable[stateType]);
        stateTable.Remove(stateType);
    }
}
