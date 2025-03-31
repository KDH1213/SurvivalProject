using UnityEngine;

[System.Serializable]
public struct GatherSaveInfo
{
    public Vector3 position;
    public Vector3 respawnPosition;
    public bool isRespawn;
    public float remainingTime;
}