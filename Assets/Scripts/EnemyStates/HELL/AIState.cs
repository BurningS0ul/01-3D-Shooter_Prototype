public enum AiStateId
{
    ChasePlayer,
    Death,
    Idle,
    Attack
}

public interface IAiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
