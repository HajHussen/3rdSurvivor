using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    PursueTargetState pursueTargetState;

    public ZombieAttackAction[] zombieAttackActions;
    public List<ZombieAttackAction> potentialAttacks;
    public ZombieAttackAction currentAttack;

    public bool hasPerformedAttack;

    private void Awake()
    {
        pursueTargetState=GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Speed", 0, .2f, Time.deltaTime);

        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Speed", 0, .2f, Time.deltaTime);
            return this;
        }
        if (!hasPerformedAttack && zombieManager.attackCooldownTimer <= 0)
        {
            if (currentAttack == null)
            {
                GetNewAttack(zombieManager);
            }
            else
            {
                AttackTarget(zombieManager);
            }
        }
        if (hasPerformedAttack)
        {
            hasPerformedAttack = false;
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
    private void GetNewAttack(ZombieManager zombieManager)
    {
        for (int i = 0; i < zombieAttackActions.Length; i++)
        {
            ZombieAttackAction zombieAttack = zombieAttackActions[i];

            if (zombieManager.distanceFromCurrentTarget <= zombieAttack.maximumAttackDistance && zombieManager.distanceFromCurrentTarget >= zombieAttack.minimumAttackDistance)
            {
                if (zombieManager.viewableAngleFromCurrentTarget <= zombieAttack.maximumAttackAngle && zombieManager.viewableAngleFromCurrentTarget >= zombieAttack.minimumAttackAngle)
                {
                    potentialAttacks.Add(zombieAttack);
                }
            }
        }
        int randomValue = Random.Range(0, potentialAttacks.Count);

        if (potentialAttacks.Count > 0)
        {
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCooldownTimer = currentAttack.attackCooldown;
            zombieManager.zombieAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);
        }
        else
        {
            Debug.LogWarning("Warning");
        }
    }
}
