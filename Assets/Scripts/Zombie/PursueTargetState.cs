using UnityEngine;

public class PursueTargetState : State
{
    AttackState attackState;

    private void Awake()
    {
        attackState = GetComponent<AttackState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.isPerformingAction )
        {
            zombieManager.animator.SetFloat("Speed", 0, .2f, Time.deltaTime);
            return this;
        }

        MoveToTarget(zombieManager);
        RotateToTarget(zombieManager);

        if (zombieManager.distanceFromCurrentTarget <= zombieManager.minimumAttackDistance)
        {
            return attackState;
        }
        else
        {
            return this;
        }
    }
    private void MoveToTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Speed", 1, .2f, Time.deltaTime);
    }

    private void RotateToTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavMesh.enabled = true;
        zombieManager.zombieNavMesh.SetDestination(zombieManager.currentTarget.transform.position);
        zombieManager.transform.LookAt(zombieManager.currentTarget.transform.position);
        //zombieManager.transform.rotation = Quaternion.Slerp(zombieManager.transform.rotation, zombieManager.zombieNavMesh.transform.rotation, zombieManager.rotationSpeed / Time.deltaTime);
    }
}
