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

public struct WaveMonsterSaveInfo
{
    public Vector3 position;
    public Quaternion rotation;
    public float hp;
    public int id;

    public WaveMonsterSaveInfo(Vector3 position, Quaternion rotation, float hp, int id)
    {
        this.position = position;
        this.rotation = rotation;
        this.hp = hp;
        this.id = id;
    }
}

public struct SpawnerSaveInfo
{
    public float currentSpawnTime;
    public int spawnCount;
    public bool isEnd;

    public SpawnerSaveInfo(float spawnTime, int count, bool isEnd)
    {
        currentSpawnTime = spawnTime;
        spawnCount = count;
        this.isEnd = isEnd;
    }
}