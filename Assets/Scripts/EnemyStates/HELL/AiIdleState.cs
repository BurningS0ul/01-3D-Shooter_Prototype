using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : IAiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }

    public void Enter(AiAgent agent) { }

    public void Exit(AiAgent agent)
    {
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 agentDir = agent.transform.forward;
        playerDirection.Normalize();
        float dotProduct = Vector3.Dot(playerDirection, agentDir);
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    public void Update(AiAgent agent) { }
}
