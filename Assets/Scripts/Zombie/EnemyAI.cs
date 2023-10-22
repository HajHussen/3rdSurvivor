using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float maxSpeed = 7;
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 2.5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float wayPointDwellTime = 2f;
    [Range(0f, 1f)]
    [SerializeField] float patrolSpeedFraction = Mathf.Clamp01(0.2f);
    Animator _animation;
    NavMeshAgent agent;

    float wayPointRange = 1f;
    private Health health;
    private GameObject target;
    private Vector3 guardPosition;
    private float wayPointReachedTime = Mathf.Infinity;
    private float lastSawTime = Mathf.Infinity;
    int currentWayPointIndex = 0;

    void Start()
    {
        guardPosition = transform.position;
        health = GetComponent<Health>();
        target = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        _animation = GetComponent<Animator>();
    }
    private void Update()
    {
        UpdateAnimator();
        if (health.isDead)
        {
            DisableNavMesh();
            return;
        }
        if (IsInAttackRange())
        {
            _animation.SetTrigger("Attack");
        }
        else
        {
            PatrolBehaviour();
        }
        UpdateTimer();
    }

    

    private void UpdateTimer()
    {
        lastSawTime += Time.deltaTime;
        wayPointReachedTime += Time.deltaTime;
    }
    private void UpdateAnimator()
    {
        Vector3 Velocity = agent.velocity;
        Vector3 LocalVelocity = transform.InverseTransformDirection(Velocity);
        float speed = LocalVelocity.z;
        _animation.SetFloat("Speed", speed);

    }

    public void StartMoveAction(Vector3 destination, float speedFraction)
    {
        MoveTo(destination, speedFraction);
    }

    public void MoveTo(Vector3 destination, float speedFraction)
    {
        agent.isStopped = false;
        agent.destination = destination;
        agent.speed = maxSpeed * speedFraction;
    }
    void PatrolBehaviour()
    {

        Vector3 nextPosition = guardPosition;
        if (patrolPath != null)
        {
            if (AtWayPoint())
            {
                wayPointReachedTime = 0;
                CycleWayPoint();
            }
            nextPosition = GetCurrentWayPoint();
        }
        if (wayPointReachedTime > wayPointDwellTime)
        {
            StartMoveAction(nextPosition, patrolSpeedFraction);
        }

    }
    private Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWayPoint(currentWayPointIndex);
    }

    private void CycleWayPoint()
    {
        currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
    }

    private bool AtWayPoint()
    {
        float distance = Vector3.Distance(GetCurrentWayPoint(), transform.position);
        return distance < wayPointRange;
    }

    private bool IsInAttackRange()
    {
        float DistanceWithPlayer = Vector3.Distance(this.transform.position, target.transform.position);
        return DistanceWithPlayer < chaseDistance;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
    }
    public void Cancel()
    {
        agent.isStopped = true;
    }
    public void DisableNavMesh()
    {
        agent.enabled = false;
    }
}
