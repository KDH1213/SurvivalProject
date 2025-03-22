[System.Serializable]

public enum StateType 
{
    None,
    Idle,
    Alert,
    Trace,
    Revert,
    AttackWait,
    Attack,
    Death,
    End
}

public enum MonsterStateType
{
    None,
    Idle,
    Move,
    Hit,
    Stun,
    Death,
    End
}