using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public float distanceFromCurrentTarget;
    public float minimumAttackDistance = 1;

    public bool isPerformingAction;

    public IdleState startingState;
    public ThirdPersonController currentTarget;
    public Animator animator;
    public NavMeshAgent zombieNavMesh;
    public Rigidbody zombieRigidBody;

    State currentState;

    private void Awake()
    {
        currentState = startingState;
        animator = GetComponent<Animator>();
        zombieRigidBody = GetComponent<Rigidbody>();
        zombieNavMesh = GetComponentInChildren<NavMeshAgent>();
    }
    private void FixedUpdate()
    {
        HandleStateMachine();
    }
    private void Update()
    {
        if (currentTarget != null)
        {
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void HandleStateMachine()
    {
        State nextState;
        if (currentState != null)
        {
            nextState = currentState.Tick(this);
            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }
}
