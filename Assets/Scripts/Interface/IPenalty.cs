public interface IPenalty
{
    void OnStartPenalty();
    void OnStopPenalty();
    bool IsOnPenalty { get; }
    SurvivalStatType PenaltyType { get; }
}