public interface IPenalty
{
    void AddPenaltyValue(float value);
    void OnStartPenalty();
    void OnStopPenalty();
    bool IsOnPenalty { get; }
    SurvivalStatType PenaltyType { get; }
}
