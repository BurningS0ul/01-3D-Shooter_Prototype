using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    public float MaxTime = 1.0f;
    public float maxDistance = 1.0f;
    public Transform playerTransform;
    NavMeshAgent agent;
    Animator anim;
    float timer = 0.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (timer < 0.0f)
        {
            float sqDistance = (playerTransform.position - agent.destination).sqrMagnitude;
            if (sqDistance > maxDistance * maxDistance)
            {
                agent.destination = playerTransform.position;
            }
            timer = MaxTime;
        }
        if (agent.hasPath)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }
}
