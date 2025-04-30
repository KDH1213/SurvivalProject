using UnityEngine;

public abstract class MonsterAttackPattern : MonoBehaviour
{
    public abstract void Enter();
    public abstract void ExecuteUpdate();
    public abstract void ExecuteFixedUpdate();
    public abstract void Exit();
}
