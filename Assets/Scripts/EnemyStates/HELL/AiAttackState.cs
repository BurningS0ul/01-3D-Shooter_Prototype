public class AiAttackState : IAiState
{
    public AiStateId GetId() {
        return AiStateId.Attack;
     }

    public void Enter(AiAgent agent) { 
        agent.ewp.SetTarget(agent.playerTransform);
    }

    public void Exit(AiAgent agent) { }

    public void Update(AiAgent agent) { 
    }
}
