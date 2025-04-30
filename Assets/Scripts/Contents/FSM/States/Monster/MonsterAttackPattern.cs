using UnityEngine;

public abstract class MonsterAttackPattern : ScriptableObject
{
    public abstract void Enter();
    public abstract void ExecuteUpdate();
    public abstract void ExecuteFixedUpdate();
    public abstract void Exit();
}
