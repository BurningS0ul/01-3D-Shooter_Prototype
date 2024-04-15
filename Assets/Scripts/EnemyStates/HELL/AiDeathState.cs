using UnityEngine;

public class AiDeathState : IAiState
{
    public Vector3 direction;

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        agent.mesh.updateWhenOffscreen = true;
    }

    public void Exit(AiAgent agent) { }

    public void Update(AiAgent agent) { }
}
