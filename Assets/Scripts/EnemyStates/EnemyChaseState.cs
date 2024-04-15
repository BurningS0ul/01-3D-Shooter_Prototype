using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager actions)
    {
        actions.anim.SetBool("Chasing", true);
    }

    public override void UpdateState(EnemyStateManager actions)
    {
        actions.timer -= Time.deltaTime;
        if (!actions.agent.enabled)
        {
            return;
        }
        actions.timer -= Time.deltaTime;
        if (!actions.agent.hasPath)
        {
            actions.agent.destination = actions.playerTransform.position;
        }

        if (actions.timer < 0.0f)
        {
            float sqDistance = (
                actions.playerTransform.position - actions.agent.destination
            ).sqrMagnitude;
            if (sqDistance > actions.maxDistance * actions.maxDistance)
            {
                if (actions.agent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    actions.agent.destination = actions.playerTransform.position;
                }
            }
            actions.timer = actions.MaxTime;

            if (actions.agent.hasPath)
            {
                actions.anim.SetFloat("Speed", actions.agent.velocity.magnitude);
            }
            else
            {
                actions.anim.SetFloat("Speed", 0);
            }
        }
    }
}
