using UnityEngine;

[System.Serializable]
public struct DamageInfo
{
    public GameObject       attackOwner;
    public Vector3          hitPoint;
    public Vector3          hitNormal;
    public Vector3          hitDirection;

    public float            damage;
    public bool             targetDeath;

    // public DebuffData       debuffData;
    // public KnockbackData    knockbackData;
    public DebuffType       debuffType;
    public float            debuffTime;
    public float            debuffProbability;
    public int              vfxID;
}
