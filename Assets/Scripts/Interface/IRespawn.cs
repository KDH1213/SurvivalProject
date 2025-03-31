public interface IRespawn
{
    UnityEngine.Vector3 RespawnPosition { get; }
    float RespawnTime { get; }
    float RemainingTime { get; }
    bool IsRespawn { get; }

    void SetRemainTime(float remainTime);
}