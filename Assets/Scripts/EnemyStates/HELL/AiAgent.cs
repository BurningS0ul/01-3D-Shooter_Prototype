using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public RagdollManager ragdoll;
    public EnemyWeaponManager ewp;
    public SkinnedMeshRenderer mesh;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<RagdollManager>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        ewp = GetComponentInChildren<EnemyWeaponManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiAttackState());

        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
