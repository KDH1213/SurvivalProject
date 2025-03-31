using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]

public struct MonsterSaveInfo
{
    public Vector3 position;
    public Vector3 respawnPosition;
    public bool isRespawn;
    public float remainingTime;
    public float hp;
}