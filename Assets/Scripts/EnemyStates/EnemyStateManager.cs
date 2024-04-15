using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public Transform playerTransform;

    public float timer;
    public float MaxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float maxSightDistance = 10.0f;

    [HideInInspector]
    public EnemyBaseState currentState;
    public EnemyChaseState Chase = new EnemyChaseState();
    public EnemyAttackState Attack = new EnemyAttackState();
    public EnemyIdleState Idle = new EnemyIdleState();

    // Add more states as needed

    void Start()
    {
        SwitchState(Idle);
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
