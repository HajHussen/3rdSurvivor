using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurrentTarget;
    public float minimumAttackDistance = 1;
    public float maximumAttackDistance = 3.5f;
    public float attackCooldownTimer;

    public Vector3 targetDirection;

    public bool isPerformingAction;

    public ZombieAnimatorManager zombieAnimatorManager;
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
        zombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
    }
    private void FixedUpdate()
    {
        HandleStateMachine();
    }
    private void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (currentTarget != null)
        {
            targetDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurrentTarget = Vector3.SignedAngle(targetDirection, transform.forward, transform.up);
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
