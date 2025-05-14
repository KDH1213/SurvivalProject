[System.Serializable]
public enum NormalSkillType
{
    Damage,
    MoveSpeed,
    AttackSpeed,
    Defence,
    HP,
    End,
}

[System.Serializable]
public enum LifeSkillType
{
    Hungur,
    Thirst,
    Fatigue,
    End
}

[System.Serializable]
public enum CraftingSkillType
{
    End,
}

[System.Serializable]
public enum SkillType
{
    NormalSkillType,
    LifeSkillType,
    CraftingSkillType,
    End,
}