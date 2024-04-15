using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager actions) { }

    public override void UpdateState(EnemyStateManager actions)
    {
        Vector3 playerDirection = actions.playerTransform.position - actions.transform.position;
        if (playerDirection.magnitude > actions.maxSightDistance)
        {
            return;
        }

        Vector3 agentDir = actions.transform.forward;
        playerDirection.Normalize();
        float dotProduct = Vector3.Dot(playerDirection, agentDir);
        if (dotProduct > 0.0f)
        {
            actions.SwitchState(actions.Chase);
        }
    }
}
