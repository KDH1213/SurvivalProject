using UnityEngine.Events;

public interface IPenalty
{
    void AddPenaltyValue(float value);
    bool IsOnPenalty { get; }
    public UnityEvent<SurvivalStatType> OnStartPenaltyEvent {get;}
    public UnityEvent<SurvivalStatType> OnEndPenaltyEvent { get; }
    SurvivalStatType PenaltyType { get; }
}
